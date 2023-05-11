using BusinessLogic.Entities;

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
    private IMemento? _memento;

    public DragLearningElement(LearningElement learningElement, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction)
    {
        LearningElement = learningElement;
        OldPositionX = oldPositionX;
        OldPositionY = oldPositionY;
        NewPositionX = newPositionX;
        NewPositionY = newPositionY;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        LearningElement.PositionX = OldPositionX;
        LearningElement.PositionY = OldPositionY;
        _memento = LearningElement.GetMemento();

        if (AnyChange()) LearningElement.UnsavedChanges = true;
        LearningElement.PositionX = NewPositionX;
        LearningElement.PositionY = NewPositionY;
        
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
        
        MappingAction.Invoke(LearningElement);
    }

    public void Redo() => Execute();
}