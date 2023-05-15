using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class DeleteLearningSpace : IDeleteLearningSpace
{
    public string Name => nameof(DeleteLearningSpace);
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public DeleteLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        LearningSpace = learningSpace;
        MappingAction = mappingAction;
    }

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

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}