using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace BusinessLogic.Commands.Element;

public class DragLearningElement : IDragLearningElement
{
    public string Name => nameof(DragLearningElement);
    internal LearningElement LearningElement { get; }
    internal double OldPositionX { get; }
    internal double OldPositionY { get; }
    internal double NewPositionX { get; }
    internal double NewPositionY { get; }
    internal readonly Action<LearningElement> MappingAction;
    private ILogger<ElementCommandFactory> Logger { get; }
    private IMemento? _memento;

    public DragLearningElement(LearningElement learningElement, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction, ILogger<ElementCommandFactory> logger)
    {
        LearningElement = learningElement;
        OldPositionX = oldPositionX;
        OldPositionY = oldPositionY;
        NewPositionX = newPositionX;
        NewPositionY = newPositionY;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        LearningElement.PositionX = OldPositionX;
        LearningElement.PositionY = OldPositionY;
        _memento = LearningElement.GetMemento();

        if (AnyChange()) LearningElement.UnsavedChanges = true;
        LearningElement.PositionX = NewPositionX;
        LearningElement.PositionY = NewPositionY;

        Logger.LogTrace("Executed drag of LearningElement {LearningElementName} ({LearningElementId}).Old position: ({OldPositionX}, {OldPositionY}) New position: ({NewPositionX}, {NewPositionY})", LearningElement.Name, LearningElement.Id, OldPositionX, OldPositionY, NewPositionX, NewPositionY);
        
        MappingAction.Invoke(LearningElement);
    }
    
    private bool AnyChange() => 
        Math.Abs(LearningElement.PositionX - NewPositionX) > 0.01 ||
        Math.Abs(LearningElement.PositionY - NewPositionY) > 0.01;

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningElement.RestoreMemento(_memento);

        Logger.LogTrace("Undone drag of LearningElement {LearningElementName} ({LearningElementId}). Restored position from ({NewPositionX}, {NewPositionY}) to: ({OldPositionX}, {OldPositionY})", LearningElement.Name, LearningElement.Id, NewPositionX, NewPositionY, OldPositionX, OldPositionY);
        
        MappingAction.Invoke(LearningElement);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DragLearningElement");
        Execute();
    }
}