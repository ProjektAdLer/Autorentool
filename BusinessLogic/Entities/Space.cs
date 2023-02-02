using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class Space : ISpace,IObjectInPathWay
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private Space()
    {
        Id = Guid.NewGuid();
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        SpaceLayout = new SpaceLayout();
        InBoundObjects = new List<IObjectInPathWay>();
        OutBoundObjects = new List<IObjectInPathWay>();
        PositionX = 0;
        PositionY = 0;
    }
    public Space(string name, string shortname, string authors, string description,
        string goals, int requiredPoints, SpaceLayout? spaceLayout = null ,double positionX = 0,
        double positionY = 0, List<IObjectInPathWay>? inBoundSpaces = null, List<IObjectInPathWay>? outBoundSpaces = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        SpaceLayout = spaceLayout ?? new SpaceLayout(Array.Empty<IElement>(), FloorPlanEnum.NoFloorPlan);
        InBoundObjects = inBoundSpaces ?? new List<IObjectInPathWay>();
        OutBoundObjects = outBoundSpaces ?? new List<IObjectInPathWay>();
        PositionX = positionX;
        PositionY = positionY;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int RequiredPoints { get; set; }
    public ISpaceLayout SpaceLayout { get; set; }
    public List<IObjectInPathWay> InBoundObjects { get; set; }
    public List<IObjectInPathWay> OutBoundObjects { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public IElement? SelectedElement { get; set; }
    public IEnumerable<IElement> ContainedElements => SpaceLayout.ContainedElements;

    public IMemento GetMemento()
    {
        return new SpaceMemento(Name, Shortname, Authors, Description, Goals, SpaceLayout, InBoundObjects, 
            OutBoundObjects, positionX: PositionX, positionY: PositionY, selectedElement: SelectedElement);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not SpaceMemento spaceMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = spaceMemento.Name;
        Shortname = spaceMemento.Shortname;
        Authors = spaceMemento.Authors;
        Description = spaceMemento.Description;
        Goals = spaceMemento.Goals;
        SpaceLayout = spaceMemento.SpaceLayout;
        InBoundObjects = spaceMemento.InBoundObjects;
        OutBoundObjects = spaceMemento.OutBoundObjects;
        PositionX = spaceMemento.PositionX;
        PositionY = spaceMemento.PositionY;
        SelectedElement = spaceMemento.SelectedElement;
    }

    private record SpaceMemento : IMemento
    {
        internal SpaceMemento(string name, string shortname, string authors, string description,
            string goals, ISpaceLayout spaceLayout, List<IObjectInPathWay> inBoundSpaces,
            List<IObjectInPathWay> outBoundSpaces, double positionX = 0, double positionY = 0,
            IElement? selectedElement = null)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Description = description;
            Goals = goals;
            SpaceLayout = spaceLayout;
            InBoundObjects = inBoundSpaces.ToList();
            OutBoundObjects = outBoundSpaces.ToList();
            PositionX = positionX;
            PositionY = positionY;
            SelectedElement = selectedElement;
        }
        
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal ISpaceLayout SpaceLayout { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        internal IElement? SelectedElement { get; }
        public List<IObjectInPathWay> InBoundObjects { get; set; }
        public List<IObjectInPathWay> OutBoundObjects { get; set; }
    }
}