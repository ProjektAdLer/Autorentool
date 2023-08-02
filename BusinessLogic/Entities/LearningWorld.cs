using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class LearningWorld : ILearningWorld, IOriginator
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LearningWorld()
    {
        Id = Guid.NewGuid();
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        SavePath = "";
        LearningSpaces = new List<ILearningSpace>();
        PathWayConditions = new List<PathWayCondition>();
        LearningPathways = new List<LearningPathway>();
        Topics = new List<Topic>();
        UnsavedChanges = false;
        UnplacedLearningElements = new List<ILearningElement>();
    }

    public LearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, string savePath = "", List<ILearningSpace>? learningSpaces = null,
        List<PathWayCondition>? pathWayConditions = null,
        List<LearningPathway>? learningPathways = null, List<Topic>? topics = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        SavePath = savePath;
        LearningSpaces = learningSpaces ?? new List<ILearningSpace>();
        PathWayConditions = pathWayConditions ?? new List<PathWayCondition>();
        LearningPathways = learningPathways ?? new List<LearningPathway>();
        Topics = topics ?? new List<Topic>();
        UnsavedChanges = true;
        UnplacedLearningElements = new List<ILearningElement>();
    }

    public List<ISelectableObjectInWorld> SelectableWorldObjects => new List<ISelectableObjectInWorld>(LearningSpaces)
        .Concat(PathWayConditions).Concat(LearningPathways).ToList();

    public Guid Id { get; private set; }
    public List<ILearningSpace> LearningSpaces { get; set; }
    public List<PathWayCondition> PathWayConditions { get; set; }

    public List<IObjectInPathWay> ObjectsInPathWays =>
        new List<IObjectInPathWay>(LearningSpaces).Concat(PathWayConditions).ToList();

    public List<LearningPathway> LearningPathways { get; set; }
    public List<Topic> Topics { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public ICollection<ILearningElement> UnplacedLearningElements { get; set; }
    public string SavePath { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges ||
               LearningSpaces.Any(space => space.UnsavedChanges) ||
               UnplacedLearningElements.Any(element => element.UnsavedChanges) ||
               PathWayConditions.Any(condition => condition.UnsavedChanges) ||
               Topics.Any(topic => topic.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public IMemento GetMemento()
    {
        return new LearningWorldMemento(Name, Shortname, Authors, Language, Description, Goals, SavePath,
            LearningSpaces,
            PathWayConditions, LearningPathways, Topics, InternalUnsavedChanges, UnplacedLearningElements);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningWorldMemento learningWorldMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        Name = learningWorldMemento.Name;
        Shortname = learningWorldMemento.Shortname;
        Authors = learningWorldMemento.Authors;
        Language = learningWorldMemento.Language;
        Description = learningWorldMemento.Description;
        Goals = learningWorldMemento.Goals;
        SavePath = learningWorldMemento.SavePath;
        LearningSpaces = learningWorldMemento.LearningSpaces;
        PathWayConditions = learningWorldMemento.PathWayConditions;
        LearningPathways = learningWorldMemento.LearningPathways;
        Topics = learningWorldMemento.Topics;
        UnsavedChanges = learningWorldMemento.UnsavedChanges;
        UnplacedLearningElements = learningWorldMemento.UnplacedLearningElements;
    }

    private record LearningWorldMemento : IMemento
    {
        internal LearningWorldMemento(string name, string shortname, string authors, string language,
            string description, string goals, string savePath, List<ILearningSpace> learningSpaces,
            List<PathWayCondition> pathWayConditions,
            List<LearningPathway> learningPathways, List<Topic> topics, bool unsavedChanges,
            IEnumerable<ILearningElement> unplacedLearningElements)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Language = language;
            Description = description;
            Goals = goals;
            SavePath = savePath;
            UnsavedChanges = unsavedChanges;
            LearningSpaces = learningSpaces.ToList();
            PathWayConditions = pathWayConditions.ToList();
            LearningPathways = learningPathways.ToList();
            Topics = topics.ToList();
            UnplacedLearningElements = unplacedLearningElements.ToList();
        }

        internal List<ILearningSpace> LearningSpaces { get; }
        internal List<PathWayCondition> PathWayConditions { get; }
        internal List<LearningPathway> LearningPathways { get; }
        internal List<Topic> Topics { get; }
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Language { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal string SavePath { get; }
        public bool UnsavedChanges { get; }
        internal List<ILearningElement> UnplacedLearningElements { get; }
    }
}