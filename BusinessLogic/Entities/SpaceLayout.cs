using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class SpaceLayout : ISpaceLayout, IOriginator
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    internal SpaceLayout()
    {
        Elements = Array.Empty<IElement>();
        FloorPlanName = FloorPlanEnum.NoFloorPlan;
    }
    
    public SpaceLayout(IElement?[] elements, FloorPlanEnum floorPlanName)
    {
        Elements = elements;
        FloorPlanName = floorPlanName;
    }
    
    public IElement?[] Elements { get; set; }
    public FloorPlanEnum FloorPlanName { get; set; }
    public IEnumerable<IElement> ContainedElements => Elements.Where(e => e != null)!;


    public IMemento GetMemento()
    {
        return new SpaceLayoutMemento(Elements, FloorPlanName);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not SpaceLayoutMemento spaceLayoutMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Elements = spaceLayoutMemento.Elements;
        FloorPlanName = spaceLayoutMemento.FloorPlanName;
    }

    private record SpaceLayoutMemento : IMemento
    {
        internal SpaceLayoutMemento(IElement?[] elements, FloorPlanEnum floorPlanName)
        {
            Elements = elements.ToArray();
            FloorPlanName = floorPlanName;
        }
        
        internal IElement?[] Elements { get; }
        internal FloorPlanEnum FloorPlanName { get; }
    }
}