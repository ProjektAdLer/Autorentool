using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Space;

public class DeleteLearningSpace : IDeleteLearningSpace
{
    private IMemento? _memento;

    public DeleteLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction, ILogger<DeleteLearningSpace> logger)
    {
        LearningWorld = learningWorld;
        LearningSpace = learningSpace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<DeleteLearningSpace> Logger { get; }
    public string Name => nameof(DeleteLearningSpace);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.UnsavedChanges = true;
        var space = LearningWorld.LearningSpaces.First(x => x.Id == LearningSpace.Id);

        foreach (var inBoundSpace in space.InBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == inBoundSpace.Id && x.TargetObject.Id == space.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }

        foreach (var outBoundSpace in space.OutBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == space.Id && x.TargetObject.Id == outBoundSpace.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }

        LearningWorld.LearningSpaces.Remove(space);

        Logger.LogTrace("Deleted LearningSpace {LearningSpaceName} ({LearningSpaceId})", LearningSpace.Name,
            LearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone deletion of LearningSpace {LearningSpaceName} ({LearningSpaceId})", LearningSpace.Name,
            LearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningSpace");
        Execute();
    }
}