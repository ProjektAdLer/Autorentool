using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places a learning element that is already contained in the layout into a different slot in that layout.
/// </summary>
public class PlaceLearningElementInLayoutFromLayout : IUndoCommand
{
    internal LearningSpace ParentSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public PlaceLearningElementInLayoutFromLayout(LearningSpace parentSpace, ILearningElement learningElement, int newNewSlotIndex,
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        LearningElement = ParentSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newNewSlotIndex;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.LearningSpaceLayout.GetMemento();

        var oldSlotIndex = Array.IndexOf(ParentSpace.LearningSpaceLayout.LearningElements, LearningElement);
        var replacedLearningElement = ParentSpace.LearningSpaceLayout.LearningElements[NewSlotIndex];
        ParentSpace.LearningSpaceLayout.LearningElements[NewSlotIndex] = LearningElement;
        ParentSpace.LearningSpaceLayout.LearningElements[oldSlotIndex] = replacedLearningElement;

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