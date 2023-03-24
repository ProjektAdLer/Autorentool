using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places a learning element that is already contained in the layout into a different slot in that layout.
/// </summary>
public class PlaceLearningElementInLayoutFromLayout : IUndoCommand
{
    public string Name => nameof(PlaceLearningElementInLayoutFromLayout);
    internal LearningSpace ParentSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public PlaceLearningElementInLayoutFromLayout(LearningSpace parentSpace, ILearningElement learningElement, int newSlotIndex,
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        LearningElement = ParentSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newSlotIndex;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.LearningSpaceLayout.GetMemento();

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

        _mappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        ParentSpace.LearningSpaceLayout.RestoreMemento(_memento);

        _mappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}