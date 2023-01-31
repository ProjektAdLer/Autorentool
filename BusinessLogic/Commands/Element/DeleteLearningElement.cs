using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class DeleteLearningElement : IUndoCommand
{
    internal LearningElement LearningElement { get; }
    internal LearningSpace ParentSpace { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public DeleteLearningElement(LearningElement learningElement, LearningSpace parentSpace,
        Action<LearningSpace> mappingAction)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();

        var element = ParentSpace.LearningSpaceLayout.LearningElements.First(x => x?.Id == LearningElement.Id);
        var elementIndex = Array.IndexOf(ParentSpace.LearningSpaceLayout.LearningElements, element);

        ParentSpace.LearningSpaceLayout.LearningElements[elementIndex] = null;

        if (element == ParentSpace.SelectedLearningElement || ParentSpace.SelectedLearningElement == null)
        {
            ParentSpace.SelectedLearningElement = ParentSpace.LearningSpaceLayout.LearningElements.LastOrDefault();
        }

        _mappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        ParentSpace.RestoreMemento(_memento);
        ParentSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        _mappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}