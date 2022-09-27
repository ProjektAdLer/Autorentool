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
        LearningElements = new List<LearningElement>();
        LearningSpaces = new List<LearningSpace>();
        UnsavedChanges = false;
    }
    public LearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningElement>? learningElements = null,
        List<LearningSpace>? learningSpaces = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElement>();
        LearningSpaces = learningSpaces ?? new List<LearningSpace>();
        UnsavedChanges = false;
    }

    public Guid Id { get; private set; }
    public List<LearningElement> LearningElements { get; set; }
    public List<LearningSpace> LearningSpaces { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public ILearningObject? SelectedLearningObject { get; set; }

    public bool UnsavedChanges { get; set; }

    public IMemento GetMemento()
    {
        return new LearningWorldMemento(Name, Shortname, Authors, Language, Description, Goals, LearningElements,
            LearningSpaces, SelectedLearningObject);
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
        LearningElements = learningWorldMemento.LearningElements;
        LearningSpaces = learningWorldMemento.LearningSpaces;
        SelectedLearningObject = learningWorldMemento.SelectedLearningObject;
    }

    private record LearningWorldMemento : IMemento
    {
        internal LearningWorldMemento(string name, string shortname, string authors, string language,
            string description, string goals, List<LearningElement> learningElements, 
            List<LearningSpace> learningSpaces, ILearningObject? selectedLearningObject = null)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Language = language;
            Description = description;
            Goals = goals;
            LearningElements = learningElements.ToList();
            LearningSpaces = learningSpaces.ToList();
            SelectedLearningObject = selectedLearningObject;
        }

        internal List<LearningElement> LearningElements { get; }
        internal List<LearningSpace> LearningSpaces { get;  }
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Language { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal ILearningObject? SelectedLearningObject { get; }
    }
}