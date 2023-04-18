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
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        //null warning override okay here as automapper must set this value after construction - n.stich
        LearningSpaceLayout = null!;
        InBoundObjects = new List<IObjectInPathWay>();
        OutBoundObjects = new List<IObjectInPathWay>();
        AssignedTopic = null;
        PositionX = 0;
        PositionY = 0;
    }
    public LearningSpace(string name, string description,
        string goals, int requiredPoints, LearningSpaceLayout? learningSpaceLayout = null, double positionX = 0,
        double positionY = 0, List<IObjectInPathWay>? inBoundSpaces = null, List<IObjectInPathWay>? outBoundSpaces = null,
        Topic? assignedTopic = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        LearningSpaceLayout = learningSpaceLayout
            ?? new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2);
        InBoundObjects = inBoundSpaces ?? new List<IObjectInPathWay>();
        OutBoundObjects = outBoundSpaces ?? new List<IObjectInPathWay>();
        AssignedTopic = assignedTopic;
        PositionX = positionX;
        PositionY = positionY;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int RequiredPoints { get; set; }
    public ILearningSpaceLayout LearningSpaceLayout { get; set; }
    public List<IObjectInPathWay> InBoundObjects { get; set; }
    public List<IObjectInPathWay> OutBoundObjects { get; set; }
    public Topic? AssignedTopic { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public ILearningElement? SelectedLearningElement { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;

    public IMemento GetMemento()
    {
        return new LearningSpaceMemento(Name, Description, Goals, RequiredPoints, LearningSpaceLayout, InBoundObjects, 
            OutBoundObjects, assignedTopic: AssignedTopic, positionX: PositionX, positionY: PositionY, selectedLearningElement: SelectedLearningElement);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningSpaceMemento learningSpaceMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = learningSpaceMemento.Name;
        Description = learningSpaceMemento.Description;
        Goals = learningSpaceMemento.Goals;
        RequiredPoints = learningSpaceMemento.RequiredPoints;
        LearningSpaceLayout = learningSpaceMemento.LearningSpaceLayout;
        InBoundObjects = learningSpaceMemento.InBoundObjects;
        OutBoundObjects = learningSpaceMemento.OutBoundObjects;
        AssignedTopic = learningSpaceMemento.AssignedTopic;
        PositionX = learningSpaceMemento.PositionX;
        PositionY = learningSpaceMemento.PositionY;
        SelectedLearningElement = learningSpaceMemento.SelectedLearningElement;
    }

    private record LearningSpaceMemento : IMemento
    {
        internal LearningSpaceMemento(string name, string description,
            string goals, int requiredPoints,  ILearningSpaceLayout learningSpaceLayout, List<IObjectInPathWay> inBoundSpaces,
            List<IObjectInPathWay> outBoundSpaces, Topic? assignedTopic, double positionX = 0, double positionY = 0,
            ILearningElement? selectedLearningElement = null)
        {
            Name = name;
            Description = description;
            Goals = goals;
            RequiredPoints = requiredPoints;
            LearningSpaceLayout = learningSpaceLayout;
            InBoundObjects = inBoundSpaces.ToList();
            OutBoundObjects = outBoundSpaces.ToList();
            AssignedTopic = assignedTopic;
            PositionX = positionX;
            PositionY = positionY;
            SelectedLearningElement = selectedLearningElement;
        }
        
        internal string Name { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int RequiredPoints { get; }
        internal ILearningSpaceLayout LearningSpaceLayout { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        internal ILearningElement? SelectedLearningElement { get; }
        public List<IObjectInPathWay> InBoundObjects { get; set; }
        public List<IObjectInPathWay> OutBoundObjects { get; set; }
        public Topic? AssignedTopic { get; set; }
    }
}