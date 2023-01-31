using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DragLearningElementToUnplaced : IUndoCommand
{
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal ILearningElement LearningElement { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _mementoWorld;
    private IMemento? _mementoSpaceLayout;

    public DragLearningElementToUnplaced(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        LearningSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningSpace.Id);
        LearningElement = LearningSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _mementoWorld = LearningWorld.GetMemento();
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();

        var oldSlot = Array.IndexOf(LearningSpace.LearningSpaceLayout.LearningElements, LearningElement);

        if (oldSlot >= 0)
        {
            LearningSpace.LearningSpaceLayout.LearningElements[oldSlot] = null;
        }

        if (LearningWorld.UnplacedLearningElements.Contains(LearningElement) == false)
        {
            LearningWorld.UnplacedLearningElements.Add(LearningElement);
        }

        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_mementoWorld == null || _mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoWorld or _mementoSpaceLayout is null");
        }

        LearningWorld.RestoreMemento(_mementoWorld);
        LearningSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}