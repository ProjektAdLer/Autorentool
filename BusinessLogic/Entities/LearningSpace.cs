using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class LearningSpace : ILearningSpace
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
        UnsavedChanges = false;
        AdvancedMode = false;
        //null warning override okay here as automapper must set this value after construction - n.stich
        LearningSpaceLayout = null!;
        InBoundObjects = new List<IObjectInPathWay>();
        OutBoundObjects = new List<IObjectInPathWay>();
        AssignedTopic = null;
        PositionX = 0;
        PositionY = 0;
    }
    public LearningSpace(string name, string description,
        string goals, int requiredPoints, Theme theme, bool advancedMode, LearningSpaceLayout? learningSpaceLayout = null,  double positionX = 0,
        double positionY = 0, List<IObjectInPathWay>? inBoundSpaces = null, List<IObjectInPathWay>? outBoundSpaces = null,
        Topic? assignedTopic = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        Theme = theme;
        AdvancedMode = advancedMode;
        UnsavedChanges = true;
        LearningSpaceLayout = learningSpaceLayout
            ?? new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X20_6L);
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
    
    public Theme Theme { get; set; }
    public bool AdvancedMode { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges ||
               ContainedLearningElements.Any(element => element.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

    public ILearningSpaceLayout LearningSpaceLayout { get; set; }
    public List<IObjectInPathWay> InBoundObjects { get; set; }
    public List<IObjectInPathWay> OutBoundObjects { get; set; }
    public Topic? AssignedTopic { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public IMemento GetMemento()
    {
        return new LearningSpaceMemento(Name, Description, Goals, RequiredPoints, Theme, AdvancedMode, LearningSpaceLayout, InBoundObjects, 
            OutBoundObjects, AssignedTopic, PositionX, PositionY, InternalUnsavedChanges);
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
        AdvancedMode = learningSpaceMemento.AdvancedMode;
        LearningSpaceLayout = learningSpaceMemento.LearningSpaceLayout;
        InBoundObjects = learningSpaceMemento.InBoundObjects;
        OutBoundObjects = learningSpaceMemento.OutBoundObjects;
        AssignedTopic = learningSpaceMemento.AssignedTopic;
        PositionX = learningSpaceMemento.PositionX;
        PositionY = learningSpaceMemento.PositionY;
        InternalUnsavedChanges = learningSpaceMemento.UnsavedChanges;
    }

    private record LearningSpaceMemento : IMemento
    {
        internal LearningSpaceMemento(string name, string description,
            string goals, int requiredPoints, Theme theme, bool advancedMode,  ILearningSpaceLayout learningSpaceLayout,
            List<IObjectInPathWay> inBoundSpaces,
            List<IObjectInPathWay> outBoundSpaces, Topic? assignedTopic, double positionX, double positionY,
            bool unsavedChanges)
        {
            Name = name;
            Description = description;
            Goals = goals;
            RequiredPoints = requiredPoints;
            Theme = theme;
            AdvancedMode = advancedMode;
            LearningSpaceLayout = learningSpaceLayout;
            InBoundObjects = inBoundSpaces.ToList();
            OutBoundObjects = outBoundSpaces.ToList();
            AssignedTopic = assignedTopic;
            PositionX = positionX;
            PositionY = positionY;
            UnsavedChanges = unsavedChanges;
        }
        
        internal string Name { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int RequiredPoints { get; }
        internal Theme Theme { get; }
        
        internal bool AdvancedMode { get; }
        internal ILearningSpaceLayout LearningSpaceLayout { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        public bool UnsavedChanges { get; }
        public List<IObjectInPathWay> InBoundObjects { get; set; }
        public List<IObjectInPathWay> OutBoundObjects { get; set; }
        public Topic? AssignedTopic { get; set; }
    }
}