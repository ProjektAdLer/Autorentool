using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteLearningSpace : IUndoCommand
{
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public DeleteLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        LearningSpace = learningSpace;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        var space = LearningWorld.LearningSpaces.First(x => x.Id == LearningSpace.Id);

        foreach (var inBoundSpace in space.InBoundSpaces)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceSpace.Id == inBoundSpace.Id && x.TargetSpace.Id == space.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }
        foreach (var outBoundSpace in space.OutBoundSpaces)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceSpace.Id == space.Id && x.TargetSpace.Id == outBoundSpace.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }
        LearningWorld.LearningSpaces.Remove(space);

        LearningWorld.SelectedLearningSpace = LearningWorld.LearningSpaces.LastOrDefault();

        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}