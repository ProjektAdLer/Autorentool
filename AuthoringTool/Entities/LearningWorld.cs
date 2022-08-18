namespace AuthoringTool.Entities;

public class LearningWorld : ILearningWorld, IOriginator
{
    internal LearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningElement>? learningElements = null,
        List<LearningSpace>? learningSpaces = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElement>();
        LearningSpaces = learningSpaces ?? new List<LearningSpace>();
    }

    public List<LearningElement> LearningElements { get; set; }
    public List<LearningSpace> LearningSpaces { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }

    public IMemento GetMemento()
    {
        return new LearningWorldMemento(Name, Shortname, Authors, Language, Description, Goals, LearningElements,
            LearningSpaces);
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
    }

    private record LearningWorldMemento : IMemento
    {
        internal LearningWorldMemento(string name, string shortname, string authors, string language,
            string description,
            string goals, List<LearningElement> learningElements, List<LearningSpace> learningSpaces)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Language = language;
            Description = description;
            Goals = goals;
            LearningElements = learningElements.ToList();
            LearningSpaces = learningSpaces.ToList();
        }

        internal List<LearningElement> LearningElements { get; }
        internal List<LearningSpace> LearningSpaces { get;  }
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Language { get; }
        internal string Description { get; }
        internal string Goals { get; }
    }
}