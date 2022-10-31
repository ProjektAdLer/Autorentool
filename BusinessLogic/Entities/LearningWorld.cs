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
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        LearningSpaces = new List<LearningSpace>();
        PathWayConditions = new List<PathWayCondition>();
        LearningPathways = new List<LearningPathway>();
        UnsavedChanges = false;
    }
    public LearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningSpace>? learningSpaces = null,
        List<PathWayCondition>? pathWayConditions = null, List<LearningPathway>? learningPathways = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningSpaces = learningSpaces ?? new List<LearningSpace>();
        PathWayConditions = pathWayConditions ?? new List<PathWayCondition>();
        LearningPathways = learningPathways ?? new List<LearningPathway>();
        UnsavedChanges = false;
    }

    public Guid Id { get; private set; }
    public List<LearningSpace> LearningSpaces { get; set; }
    public List<PathWayCondition> PathWayConditions { get; set; }
    public List<IObjectInPathWay> PathWayObjects => new List<IObjectInPathWay>(LearningSpaces).Concat(PathWayConditions).ToList();
    public List<ISelectableObjectInWorld> SelectableWorldObjects => new List<ISelectableObjectInWorld>(LearningSpaces).
        Concat(PathWayConditions).Concat(LearningPathways).ToList();
    public List<LearningPathway> LearningPathways { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public ISelectableObjectInWorld? SelectedLearningObject { get; set; }

    public bool UnsavedChanges { get; set; }

    public IMemento GetMemento()
    {
        return new LearningWorldMemento(Name, Shortname, Authors, Language, Description, Goals, LearningSpaces,
            PathWayConditions, LearningPathways, SelectedLearningObject);
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
        LearningSpaces = learningWorldMemento.LearningSpaces;
        PathWayConditions = learningWorldMemento.PathWayConditions;
        LearningPathways = learningWorldMemento.LearningPathways;
        SelectedLearningObject = learningWorldMemento.SelectedLearningSpace;
    }

    private record LearningWorldMemento : IMemento
    {
        internal LearningWorldMemento(string name, string shortname, string authors, string language,
            string description, string goals, List<LearningSpace> learningSpaces, List<PathWayCondition> pathWayConditions,
            List<LearningPathway> learningPathways, ISelectableObjectInWorld? selectedLearningSpace = null)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Language = language;
            Description = description;
            Goals = goals;
            LearningSpaces = learningSpaces.ToList();
            PathWayConditions = pathWayConditions.ToList();
            LearningPathways = learningPathways.ToList();
            SelectedLearningSpace = selectedLearningSpace;
        }

        internal List<LearningSpace> LearningSpaces { get;  }
        internal List<PathWayCondition> PathWayConditions { get;  }
        internal List<LearningPathway> LearningPathways { get;  }
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Language { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal ISelectableObjectInWorld? SelectedLearningSpace { get; }
    }
}