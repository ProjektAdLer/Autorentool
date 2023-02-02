using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class World : IWorld, IOriginator
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private World()
    {
        Id = Guid.NewGuid();
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        Spaces = new List<Space>();
        PathWayConditions = new List<PathWayCondition>();
        Pathways = new List<Pathway>();
        UnsavedChanges = false;
        UnplacedElements = new List<IElement>();
    }
    public World(string name, string shortname, string authors, string language, string description,
        string goals, List<Space>? spaces = null,
        List<PathWayCondition>? pathWayConditions = null, List<Pathway>? pathways = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        Spaces = spaces ?? new List<Space>();
        PathWayConditions = pathWayConditions ?? new List<PathWayCondition>();
        Pathways = pathways ?? new List<Pathway>();
        UnsavedChanges = false;
        UnplacedElements = new List<IElement>();
    }

    public Guid Id { get; private set; }
    public List<Space> Spaces { get; set; }
    public List<PathWayCondition> PathWayConditions { get; set; }
    public List<IObjectInPathWay> ObjectsInPathWays => new List<IObjectInPathWay>(Spaces).Concat(PathWayConditions).ToList();
    public List<ISelectableObjectInWorld> SelectableWorldObjects => new List<ISelectableObjectInWorld>(Spaces).
        Concat(PathWayConditions).Concat(Pathways).ToList();
    public List<Pathway> Pathways { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public ICollection<IElement> UnplacedElements { get; set; } 
    public ISelectableObjectInWorld? SelectedObject { get; set; }

    public bool UnsavedChanges { get; set; }

    public IMemento GetMemento()
    {
        return new WorldMemento(Name, Shortname, Authors, Language, Description, Goals, Spaces,
            PathWayConditions, Pathways, UnplacedElements, SelectedObject);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not WorldMemento worldMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = worldMemento.Name;
        Shortname = worldMemento.Shortname;
        Authors = worldMemento.Authors;
        Language = worldMemento.Language;
        Description = worldMemento.Description;
        Goals = worldMemento.Goals;
        Spaces = worldMemento.Spaces;
        PathWayConditions = worldMemento.PathWayConditions;
        Pathways = worldMemento.Pathways;
        UnplacedElements = worldMemento.UnplacedElements;
        SelectedObject = worldMemento.SelectedSpace;
    }

    private record WorldMemento : IMemento
    {
        internal WorldMemento(string name, string shortname, string authors, string language,
            string description, string goals, List<Space> spaces, List<PathWayCondition> pathWayConditions,
            List<Pathway> pathways, IEnumerable<IElement> unplacedElements, ISelectableObjectInWorld? selectedSpace = null)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Language = language;
            Description = description;
            Goals = goals;
            Spaces = spaces.ToList();
            PathWayConditions = pathWayConditions.ToList();
            Pathways = pathways.ToList();
            UnplacedElements = unplacedElements.ToList();
            SelectedSpace = selectedSpace;
        }

        internal List<Space> Spaces { get;  }
        internal List<PathWayCondition> PathWayConditions { get;  }
        internal List<Pathway> Pathways { get;  }
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Language { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal List<IElement> UnplacedElements { get; }
        internal ISelectableObjectInWorld? SelectedSpace { get; }
    }
}