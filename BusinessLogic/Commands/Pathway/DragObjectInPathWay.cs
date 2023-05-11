using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class DragObjectInPathWay : IDragObjectInPathWay
{
    public string Name => nameof(DragObjectInPathWay);
    internal IObjectInPathWay LearningObject { get; }
    internal double OldPositionX { get; }
    internal double OldPositionY { get; }
    internal double NewPositionX { get; }
    internal double NewPositionY { get; }
    internal Action<IObjectInPathWay> MappingAction { get; }
    private IMemento? _memento;

    public DragObjectInPathWay(IObjectInPathWay learningObject, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction)
    {
        LearningObject = learningObject;
        OldPositionX = oldPositionX;
        OldPositionY = oldPositionY;
        NewPositionX = newPositionX;
        NewPositionY = newPositionY;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        LearningObject.PositionX = OldPositionX;
        LearningObject.PositionY = OldPositionY;
        _memento = LearningObject.GetMemento();

        if(AnyChanges()) LearningObject.UnsavedChanges = true;
        LearningObject.PositionX = NewPositionX;
        LearningObject.PositionY = NewPositionY;
        
        MappingAction.Invoke(LearningObject);
    }

    private bool AnyChanges() =>
        Math.Abs(LearningObject.PositionX - NewPositionX) > 0.01 ||
        Math.Abs(LearningObject.PositionY - NewPositionY) > 0.01;

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningObject.RestoreMemento(_memento);
        
        MappingAction.Invoke(LearningObject);
    }

    public void Redo() => Execute();
}