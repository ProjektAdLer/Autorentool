using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places an unplaced learning element in a slot in a layout.
/// </summary>
public class PlaceLearningElementInLayoutFromUnplaced : IUndoCommand
{
    public string Name => nameof(PlaceLearningElementInLayoutFromUnplaced);
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _mementoWorld;
    private IMemento? _mementoSpaceLayout;

    public PlaceLearningElementInLayoutFromUnplaced(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        LearningSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningSpace.Id);
        LearningElement = LearningWorld.UnplacedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newSlotIndex;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _mementoWorld = LearningWorld.GetMemento();
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();

        if (LearningWorld.UnplacedLearningElements.Contains(LearningElement))
        {
            LearningWorld.UnplacedLearningElements.Remove(LearningElement);
        }

        var oldElement = LearningSpace.LearningSpaceLayout.LearningElements.ContainsKey(NewSlotIndex)
            ? LearningSpace.LearningSpaceLayout.LearningElements[NewSlotIndex]
            : null;
        if (oldElement != null)
        {
            if (LearningWorld.UnplacedLearningElements.Contains(oldElement) == false)
            {
                LearningWorld.UnplacedLearningElements.Add(oldElement);
            }
        }

        LearningSpace.LearningSpaceLayout.LearningElements[NewSlotIndex] = LearningElement;
        LearningSpace.SelectedLearningElement = LearningElement;

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