using BusinessLogic.Entities;
using BusinessLogic.Entities.FloorPlans;
using Shared;

namespace BusinessLogic.Commands.Layout;

public class ChangeSpaceLayout : IUndoCommand
{
    internal Entities.Space Space { get; }
    internal FloorPlanEnum FloorPlanName { get; }
    private readonly Action<Entities.Space> _mappingAction;
    private IMemento? _memento;

    public ChangeSpaceLayout(Entities.Space space, FloorPlanEnum floorPlanName, Action<Entities.Space> mappingAction)
    {
        Space = space;
        FloorPlanName = floorPlanName;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = Space.SpaceLayout.GetMemento();

        var newElementArray = new IElement?[FloorPlanProvider.GetFloorPlan(FloorPlanName).Capacity];
        for (int i = 0; i < Math.Min(Space.SpaceLayout.Elements.Length, newElementArray.Length); i++)
        {
            newElementArray[i] = Space.SpaceLayout.Elements[i];
        }
        Space.SpaceLayout.Elements = newElementArray;
        Space.SpaceLayout.FloorPlanName = FloorPlanName;
        
        _mappingAction.Invoke(Space);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        Space.SpaceLayout.RestoreMemento(_memento);
        
        _mappingAction.Invoke(Space);
    }

    public void Redo() => Execute();
}