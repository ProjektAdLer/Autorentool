using JetBrains.Annotations;

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
        LearningElements = new List<LearningElement>();
        InBoundObjects = new List<IObjectInPathWay>();
        OutBoundObjects = new List<IObjectInPathWay>();
        AssignedTopic = null;
        PositionX = 0;
        PositionY = 0;
    }
    public LearningSpace(string name, string shortname, string authors, string description,
        string goals, int requiredPoints, List<LearningElement>? learningElements = null ,double positionX = 0,
        double positionY = 0, List<IObjectInPathWay>? inBoundSpaces = null, List<IObjectInPathWay>? outBoundSpaces = null,
        Topic? assignedTopic = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        LearningElements = learningElements ?? new List<LearningElement>();
        InBoundObjects = inBoundSpaces ?? new List<IObjectInPathWay>();
        OutBoundObjects = outBoundSpaces ?? new List<IObjectInPathWay>();
        AssignedTopic = assignedTopic;
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
    public List<LearningElement> LearningElements { get; set; }
    public List<IObjectInPathWay> InBoundObjects { get; set; }
    public List<IObjectInPathWay> OutBoundObjects { get; set; }
    public Topic? AssignedTopic { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public ILearningElement? SelectedLearningElement { get; set; }

    public IMemento GetMemento()
    {
        return new LearningSpaceMemento(Name, Shortname, Authors, Description, Goals, RequiredPoints, LearningElements, InBoundObjects, 
            OutBoundObjects, assignedTopic:AssignedTopic, positionX: PositionX, positionY: PositionY, selectedLearningElement: SelectedLearningElement);
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
        RequiredPoints = learningSpaceMemento.RequiredPoints;
        LearningElements = learningSpaceMemento.LearningElements;
        InBoundObjects = learningSpaceMemento.InBoundObjects;
        OutBoundObjects = learningSpaceMemento.OutBoundObjects;
        AssignedTopic = learningSpaceMemento.AssignedTopic;
        PositionX = learningSpaceMemento.PositionX;
        PositionY = learningSpaceMemento.PositionY;
        SelectedLearningElement = learningSpaceMemento.SelectedLearningElement;
    }

    private record LearningSpaceMemento : IMemento
    {
        internal LearningSpaceMemento(string name, string shortname, string authors, string description,
            string goals, int requiredPoints,  List<LearningElement> learningElements, List<IObjectInPathWay> inBoundSpaces,
            List<IObjectInPathWay> outBoundSpaces, Topic? assignedTopic, double positionX = 0, double positionY = 0,
            ILearningElement? selectedLearningElement = null)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Description = description;
            Goals = goals;
            RequiredPoints = requiredPoints;
            LearningElements = learningElements.ToList();
            InBoundObjects = inBoundSpaces.ToList();
            OutBoundObjects = outBoundSpaces.ToList();
            AssignedTopic = assignedTopic;
            PositionX = positionX;
            PositionY = positionY;
            SelectedLearningElement = selectedLearningElement;
        }
        
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int RequiredPoints { get; }
        internal List<LearningElement> LearningElements { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        internal ILearningElement? SelectedLearningElement { get; }
        public List<IObjectInPathWay> InBoundObjects { get; set; }
        public List<IObjectInPathWay> OutBoundObjects { get; set; }
        public Topic? AssignedTopic { get; set; }
    }
}