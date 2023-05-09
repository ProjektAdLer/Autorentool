using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places an unplaced learning element in a slot in a layout.
/// </summary>
public class PlaceLearningElementInLayoutFromUnplaced : IPlaceLearningElementInLayoutFromUnplaced
{
    public string Name => nameof(PlaceLearningElementInLayoutFromUnplaced);
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace LearningSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _mementoWorld;
    private IMemento? _mementoSpace;
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
        _mementoSpace = LearningSpace.GetMemento();
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();
        
        LearningSpace.UnsavedChanges = true;
        LearningWorld.UnsavedChanges = true;

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

        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_mementoWorld == null)
        {
            throw new InvalidOperationException("_mementoWorld is null");
        }
        if(_mementoSpace == null)
        {
            throw new InvalidOperationException("_mementoSpace is null");
        }
        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        LearningWorld.RestoreMemento(_mementoWorld);
        LearningSpace.RestoreMemento(_mementoSpace);
        LearningSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}