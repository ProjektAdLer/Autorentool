using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places a learning element that is already contained in the layout into a different slot in that layout.
/// </summary>
public class PlaceLearningElementInLayoutFromLayout : IPlaceLearningElementInLayoutFromLayout
{
    public string Name => nameof(PlaceLearningElementInLayoutFromLayout);
    internal LearningSpace ParentSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private IMemento? _mementoLayout;
    private IMemento? _mementoSpace;

    public PlaceLearningElementInLayoutFromLayout(LearningSpace parentSpace, ILearningElement learningElement, int newSlotIndex,
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        LearningElement = ParentSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newSlotIndex;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _mementoLayout = ParentSpace.LearningSpaceLayout.GetMemento();
        _mementoSpace = ParentSpace.GetMemento();

        ParentSpace.UnsavedChanges = true;

        var kvP = ParentSpace.LearningSpaceLayout.LearningElements
            .First(kvP => kvP.Value.Equals(LearningElement));
        var oldSlotIndex = kvP.Key;
        var replacedLearningElement = ParentSpace.LearningSpaceLayout.LearningElements.ContainsKey(NewSlotIndex)
            ? ParentSpace.LearningSpaceLayout.LearningElements[NewSlotIndex]
            : null;
        ParentSpace.LearningSpaceLayout.LearningElements[NewSlotIndex] = LearningElement;
        if (replacedLearningElement != null)
            ParentSpace.LearningSpaceLayout.LearningElements[oldSlotIndex] = replacedLearningElement;
        else 
            ParentSpace.LearningSpaceLayout.LearningElements.Remove(oldSlotIndex);

        MappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_mementoLayout == null)
        {
            throw new InvalidOperationException("_mementoLayout is null");
        }

        if (_mementoSpace == null)
        {
            throw new InvalidOperationException("_mementoSpace is null");
        }

        ParentSpace.LearningSpaceLayout.RestoreMemento(_mementoLayout);
        ParentSpace.RestoreMemento(_mementoSpace);

        MappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}