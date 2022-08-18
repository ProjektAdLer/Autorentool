using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

public class LearningElement : ILearningElement, IOriginator
{
    internal LearningElement(string name, string shortname,  ILearningElementParent? parent, LearningContent? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload = 0,
        double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        LearningContent = learningContent;
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        PositionX = positionX;
        PositionY = positionY;
        Parent = parent;
    }

    public string Name { get; set; }
    public string Shortname { get; set; }
    public ILearningElementParent? Parent { get; set; }
    public LearningContent LearningContent { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int Workload { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }

    public IMemento GetMemento()
    {
        return new LearningElementMemento(Name, Shortname, LearningContent, Authors, Description, Goals, Workload, Difficulty,
            Parent, PositionX, PositionY);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningElementMemento learningElementMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = learningElementMemento.Name;
        Shortname = learningElementMemento.Shortname;
        LearningContent = learningElementMemento.Content;
        Authors = learningElementMemento.Authors;
        Description = learningElementMemento.Description;
        Goals = learningElementMemento.Goals;
        Workload = learningElementMemento.Workload;
        Difficulty = learningElementMemento.Difficulty;
        Parent = learningElementMemento.Parent;
        PositionX = learningElementMemento.PositionX;
        PositionY = learningElementMemento.PositionY;
    }

    private record LearningElementMemento : IMemento
    {
        internal LearningElementMemento(string name, string shortname, LearningContent? content, string authors,
            string description, string goals, int workload, LearningElementDifficultyEnum difficulty, ILearningElementParent? parent,
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
            Parent = parent;
            PositionX = positionX;
            PositionY = positionY;
        }
        
        internal string Name { get; }
        internal string Shortname { get; }
        internal ILearningElementParent? Parent { get; }
        internal LearningContent? Content { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int Workload { get; }
        internal LearningElementDifficultyEnum Difficulty { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
    }
}

