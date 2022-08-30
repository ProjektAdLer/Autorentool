using Shared;

namespace BusinessLogic.Entities;

public class LearningElement : ILearningElement, IOriginator
{
    internal LearningElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload = 0,
        double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Content = content;
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        PositionX = positionX;
        PositionY = positionY;
        ParentName = parentName;
    }

    public string Name { get; set; }
    public string Shortname { get; set; }
    public LearningContent? Content { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int Workload { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string? ParentName { get; set; }

    public IMemento GetMemento()
    {
        return new LearningElementMemento(Name, Shortname, Content, Authors, Description, Goals, Workload, Difficulty,
            ParentName, PositionX, PositionY);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningElementMemento learningElementMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = learningElementMemento.Name;
        Shortname = learningElementMemento.Shortname;
        Content = learningElementMemento.Content;
        Authors = learningElementMemento.Authors;
        Description = learningElementMemento.Description;
        Goals = learningElementMemento.Goals;
        Workload = learningElementMemento.Workload;
        Difficulty = learningElementMemento.Difficulty;
        ParentName = learningElementMemento.ParentName;
        PositionX = learningElementMemento.PositionX;
        PositionY = learningElementMemento.PositionY;
    }

    private record LearningElementMemento : IMemento
    {
        internal LearningElementMemento(string name, string shortname, LearningContent? content, string authors,
            string description, string goals, int workload, LearningElementDifficultyEnum difficulty, string? parentName,
            double positionX = 0, double positionY = 0)
        {
            Name = name;
            Shortname = shortname;
            Content = content;
            Authors = authors;
            Description = description;
            Goals = goals;
            Workload = workload;
            Difficulty = difficulty;
            ParentName = parentName;
            PositionX = positionX;
            PositionY = positionY;
        }
        
        internal string Name { get; }
        internal string Shortname { get; }
        internal LearningContent? Content { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int Workload { get; }
        internal LearningElementDifficultyEnum Difficulty { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        internal string? ParentName { get; }
    }
}

