using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Pathway;

public class DragObjectInPathWay : IDragObjectInPathWay
{
    private IMemento? _memento;

    public DragObjectInPathWay(IObjectInPathWay learningObject, double oldPositionX, double oldPositionY,
        double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction,
        ILogger<DragObjectInPathWay> logger)
    {
        LearningObject = learningObject;
        OldPositionX = oldPositionX;
        OldPositionY = oldPositionY;
        NewPositionX = newPositionX;
        NewPositionY = newPositionY;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal IObjectInPathWay LearningObject { get; }
    internal double OldPositionX { get; }
    internal double OldPositionY { get; }
    internal double NewPositionX { get; }
    internal double NewPositionY { get; }
    internal Action<IObjectInPathWay> MappingAction { get; }
    private ILogger<DragObjectInPathWay> Logger { get; }
    public string Name => nameof(DragObjectInPathWay);

    public void Execute()
    {
        LearningObject.PositionX = OldPositionX;
        LearningObject.PositionY = OldPositionY;
        _memento = LearningObject.GetMemento();

        if (AnyChanges()) LearningObject.UnsavedChanges = true;
        LearningObject.PositionX = NewPositionX;
        LearningObject.PositionY = NewPositionY;

        Logger.LogTrace(
            "Dragged LearningObject {LearningObjectId} (from position ({OldPositionX}, {OldPositionY}) to position ({NewPositionX}, {NewPositionY})",
            LearningObject.Id, OldPositionX, OldPositionY, NewPositionX, NewPositionY);

        MappingAction.Invoke(LearningObject);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningObject.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone dragging of LearningObject {LearningObjectId}. Restored position to ({OldPositionX}, {OldPositionY})",
            LearningObject.Id, OldPositionX, OldPositionY);

        MappingAction.Invoke(LearningObject);
    }

    public void Redo() => Execute();

    private bool AnyChanges() =>
        Math.Abs(LearningObject.PositionX - NewPositionX) > 0.01 ||
        Math.Abs(LearningObject.PositionY - NewPositionY) > 0.01;
}