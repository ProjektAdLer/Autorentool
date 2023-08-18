using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public class DeleteAdvancedLearningSpace : IDeleteAdvancedLearningSpace
{
    private IMemento? _memento;

    public DeleteAdvancedLearningSpace(LearningWorld learningWorld, Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction, ILogger<DeleteAdvancedLearningSpace> logger)
    {
        LearningWorld = learningWorld;
        AdvancedLearningSpace = advancedLearningSpace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal Entities.AdvancedLearningSpaces.AdvancedLearningSpace AdvancedLearningSpace { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<DeleteAdvancedLearningSpace> Logger { get; }
    public string Name => nameof(DeleteAdvancedLearningSpace);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        var advancedSpace = LearningWorld.LearningSpaces.First(x => x.Id == AdvancedLearningSpace.Id);
        
        foreach (var inBoundSpace in advancedSpace.InBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == inBoundSpace.Id && x.TargetObject.Id == advancedSpace.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }

        foreach (var outBoundSpace in advancedSpace.OutBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == advancedSpace.Id && x.TargetObject.Id == outBoundSpace.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }

        LearningWorld.LearningSpaces.Remove(advancedSpace);

        Logger.LogTrace("Deleted Advanced LearningSpace {AdvancedLearningSpaceName} ({AdvancedLearningSpaceId})", AdvancedLearningSpace.Name,
            AdvancedLearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone deletion of Advanced LearningSpace {AdvancedLearningSpaceName} ({AdvancedLearningSpaceId})", AdvancedLearningSpace.Name,
            AdvancedLearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteAdvancedLearningSpace");
        Execute();
    }
}