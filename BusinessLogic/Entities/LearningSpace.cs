using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class LearningSpace : ILearningSpace,IObjectInPathWay
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LearningSpace()
    {
        Id = Guid.NewGuid();
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        //null warning override okay here as automapper must set this value after construction - n.stich
        LearningSpaceLayout = null!;
        InBoundObjects = new List<IObjectInPathWay>();
        OutBoundObjects = new List<IObjectInPathWay>();
        PositionX = 0;
        PositionY = 0;
    }
    public LearningSpace(string name, string shortname, string authors, string description,
        string goals, int requiredPoints, LearningSpaceLayout? learningSpaceLayout = null, double positionX = 0,
        double positionY = 0, List<IObjectInPathWay>? inBoundSpaces = null, List<IObjectInPathWay>? outBoundSpaces = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        LearningSpaceLayout = learningSpaceLayout
            ?? new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2);
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
    public ILearningSpaceLayout LearningSpaceLayout { get; set; }
    public List<IObjectInPathWay> InBoundObjects { get; set; }
    public List<IObjectInPathWay> OutBoundObjects { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public ILearningElement? SelectedLearningElement { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;

    public IMemento GetMemento()
    {
        return new LearningSpaceMemento(Name, Shortname, Authors, Description, Goals, LearningSpaceLayout, InBoundObjects, 
            OutBoundObjects, positionX: PositionX, positionY: PositionY, selectedLearningElement: SelectedLearningElement);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningSpaceMemento learningSpaceMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = learningSpaceMemento.Name;
        Shortname = learningSpaceMemento.Shortname;
        Authors = learningSpaceMemento.Authors;
        Description = learningSpaceMemento.Description;
        Goals = learningSpaceMemento.Goals;
        LearningSpaceLayout = learningSpaceMemento.LearningSpaceLayout;
        InBoundObjects = learningSpaceMemento.InBoundObjects;
        OutBoundObjects = learningSpaceMemento.OutBoundObjects;
        PositionX = learningSpaceMemento.PositionX;
        PositionY = learningSpaceMemento.PositionY;
        SelectedLearningElement = learningSpaceMemento.SelectedLearningElement;
    }

    private record LearningSpaceMemento : IMemento
    {
        internal LearningSpaceMemento(string name, string shortname, string authors, string description,
            string goals, ILearningSpaceLayout learningSpaceLayout, List<IObjectInPathWay> inBoundSpaces,
            List<IObjectInPathWay> outBoundSpaces, double positionX = 0, double positionY = 0,
            ILearningElement? selectedLearningElement = null)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Description = description;
            Goals = goals;
            LearningSpaceLayout = learningSpaceLayout;
            InBoundObjects = inBoundSpaces.ToList();
            OutBoundObjects = outBoundSpaces.ToList();
            PositionX = positionX;
            PositionY = positionY;
            SelectedLearningElement = selectedLearningElement;
        }
        
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal ILearningSpaceLayout LearningSpaceLayout { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        internal ILearningElement? SelectedLearningElement { get; }
        public List<IObjectInPathWay> InBoundObjects { get; set; }
        public List<IObjectInPathWay> OutBoundObjects { get; set; }
    }
}