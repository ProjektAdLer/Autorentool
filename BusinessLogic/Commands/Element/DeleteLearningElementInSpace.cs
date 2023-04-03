﻿using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class DeleteLearningElementInSpace : IUndoCommand
{
    public string Name => nameof(DeleteLearningElementInSpace);
    internal LearningElement LearningElement { get; }
    internal LearningSpace ParentSpace { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public DeleteLearningElementInSpace(LearningElement learningElement, LearningSpace parentSpace,
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

        var kvP = ParentSpace.LearningSpaceLayout.LearningElements.First(x => x.Value.Id == LearningElement.Id);
        
        ParentSpace.LearningSpaceLayout.LearningElements.Remove(kvP.Key);

        if (kvP.Value == ParentSpace.SelectedLearningElement || ParentSpace.SelectedLearningElement == null)
        {
            ParentSpace.SelectedLearningElement = ParentSpace.LearningSpaceLayout.LearningElements.Values.LastOrDefault();
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