using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities.AdvancedLearningSpaces;

public class AdvancedLearningSpace : IAdvancedLearningSpace
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private AdvancedLearningSpace()
    {
        // LearningSpaceLayout = new LearningSpaceLayout( new Dictionary<int, ILearningElement>(), FloorPlanEnum.L_32X31_10L);
        LearningSpaceLayout = null!;
        Id = Guid.NewGuid();
        Name = "";
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        UnsavedChanges = false;
        //null warning override okay here as automapper must set this value after construction - n.stich
        AdvancedLearningSpaceLayout =  new AdvancedLearningSpaceLayout();
        InBoundObjects = new List<IObjectInPathWay>();
        OutBoundObjects = new List<IObjectInPathWay>();
        AssignedTopic = null;
        PositionX = 0;
        PositionY = 0;
    }

    public AdvancedLearningSpace(string name, string description,
        string goals, int requiredPoints, Theme theme, AdvancedLearningSpaceLayout? advancedLearningSpaceLayout = null,  double positionX = 0,
        double positionY = 0, List<IObjectInPathWay>? inBoundSpaces = null, List<IObjectInPathWay>? outBoundSpaces = null,
        Topic? assignedTopic = null)
    {
        // LearningSpaceLayout = new LearningSpaceLayout( new Dictionary<int, ILearningElement>(), FloorPlanEnum.L_32X31_10L);
        LearningSpaceLayout = null!;
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        Theme = theme;
        UnsavedChanges = true;
        AdvancedLearningSpaceLayout = advancedLearningSpaceLayout
                                      ?? new AdvancedLearningSpaceLayout();
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
    public ILearningSpaceLayout LearningSpaceLayout { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges ||
               ContainedLearningElements.Any(element => element.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

    public IAdvancedLearningSpaceLayout AdvancedLearningSpaceLayout { get; set; }
    public List<IObjectInPathWay> InBoundObjects { get; set; }
    public List<IObjectInPathWay> OutBoundObjects { get; set; }
    public Topic? AssignedTopic { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => AdvancedLearningSpaceLayout.ContainedLearningElements;

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public IMemento GetMemento()
    {
        return new AdvancedLearningSpaceMemento(Name, Description, Goals, RequiredPoints, Theme, AdvancedLearningSpaceLayout,
            InBoundObjects,
            OutBoundObjects, AssignedTopic, PositionX, PositionY, InternalUnsavedChanges);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AdvancedLearningSpaceMemento advancedLearningSpaceMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        Name = advancedLearningSpaceMemento.Name;
        Description = advancedLearningSpaceMemento.Description;
        Goals = advancedLearningSpaceMemento.Goals;
        RequiredPoints = advancedLearningSpaceMemento.RequiredPoints;
        AdvancedLearningSpaceLayout = advancedLearningSpaceMemento.AdvancedLearningSpaceLayout;
        InBoundObjects = advancedLearningSpaceMemento.InBoundObjects;
        OutBoundObjects = advancedLearningSpaceMemento.OutBoundObjects;
        AssignedTopic = advancedLearningSpaceMemento.AssignedTopic;
        PositionX = advancedLearningSpaceMemento.PositionX;
        PositionY = advancedLearningSpaceMemento.PositionY;
        InternalUnsavedChanges = advancedLearningSpaceMemento.UnsavedChanges;
    }

    private record AdvancedLearningSpaceMemento : IMemento
    {
        internal AdvancedLearningSpaceMemento(string name, string description,
            string goals, int requiredPoints, Theme theme,  IAdvancedLearningSpaceLayout advancedLearningSpaceLayout,
            List<IObjectInPathWay> inBoundSpaces,
            List<IObjectInPathWay> outBoundSpaces, Topic? assignedTopic, double positionX, double positionY,
            bool unsavedChanges)
        {
            Name = name;
            Description = description;
            Goals = goals;
            RequiredPoints = requiredPoints;
            Theme = theme;
            AdvancedLearningSpaceLayout = advancedLearningSpaceLayout;
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

        // ReSharper disable once UnusedAutoPropertyAccessor.Local TODO: use Theme property in future
        internal Theme Theme { get; }
        internal IAdvancedLearningSpaceLayout AdvancedLearningSpaceLayout { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        public bool UnsavedChanges { get; }
        public List<IObjectInPathWay> InBoundObjects { get; set; }
        public List<IObjectInPathWay> OutBoundObjects { get; set; }
        public Topic? AssignedTopic { get; set; }
    }
}