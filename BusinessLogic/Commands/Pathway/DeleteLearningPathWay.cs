using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Pathway;

public class DeleteLearningPathWay : IDeleteLearningPathWay
{
    public string Name => nameof(DeleteLearningPathWay);
    internal LearningWorld LearningWorld { get; }
    internal LearningPathway LearningPathway { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;
    private ILogger<PathwayCommandFactory> Logger { get; }
    
    public DeleteLearningPathWay(LearningWorld learningWorld, LearningPathway learningPathway,
        Action<LearningWorld> mappingAction, ILogger<PathwayCommandFactory> logger)
    {
        LearningWorld = learningWorld;
        LearningPathway = learningWorld.LearningPathways.Single(x => x.Id == learningPathway.Id);
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        LearningWorld.LearningPathways.Remove(LearningPathway);
        
        Logger.LogTrace("Deleted LearningPathway from {SourceObjectId} to {TargetObjectId} in LearningWorld {LearningWorldName} {LearningWorldId}", LearningPathway.SourceObject.Id, LearningPathway.TargetObject.Id, LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);
        
        Logger.LogTrace("Undone deletion of LearningPathway from {SourceObjectId} to {TargetObjectId} in LearningWorld {LearningWorldName} {LearningWorldId}", LearningPathway.SourceObject.Id, LearningPathway.TargetObject.Id, LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningPathWay");
        Execute();
    }
}