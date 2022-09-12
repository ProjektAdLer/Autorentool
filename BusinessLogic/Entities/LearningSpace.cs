using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class LearningSpace : ILearningSpace, IOriginator
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LearningSpace()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        LearningElements = new List<LearningElement>();
        PositionX = 0;
        PositionY = 0;
    }
    public LearningSpace(string name, string shortname, string authors, string description,
        string goals, List<LearningElement>? learningElements = null, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElement>();
        PositionX = positionX;
        PositionY = positionY;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public List<LearningElement> LearningElements { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }

    public IMemento GetMemento()
    {
        return new LearningSpaceMemento(Name, Shortname, Authors, Description, Goals, LearningElements, PositionX,
            PositionY);
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
        LearningElements = learningSpaceMemento.LearningElements;
        PositionX = learningSpaceMemento.PositionX;
        PositionY = learningSpaceMemento.PositionY;
    }

    private record LearningSpaceMemento : IMemento
    {
        internal LearningSpaceMemento(string name, string shortname, string authors, string description,
            string goals, List<LearningElement> learningElements, double positionX = 0, double positionY = 0)
        {
            Name = name;
            Shortname = shortname;
            Authors = authors;
            Description = description;
            Goals = goals;
            LearningElements = learningElements.ToList();
            PositionX = positionX;
            PositionY = positionY;
        }
        
        internal string Name { get; }
        internal string Shortname { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal List<LearningElement> LearningElements { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
    }
}