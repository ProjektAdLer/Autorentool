using JetBrains.Annotations;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public class LearningElement : ILearningElement, IOriginator
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    protected LearningElement()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - n.stich
        LearningContent = null!;
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.None;
        Workload = 0;
        PositionX = 0;
        PositionY = 0;
        Parent = null;
    }

    public LearningElement(string name, string shortname, LearningContent learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty,
        ILearningElementParent? parent = null, int workload = 0,
        double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
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

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; private set; }
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
        internal LearningElementMemento(string name, string shortname, LearningContent content, string authors,
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
        internal LearningContent Content { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int Workload { get; }
        internal LearningElementDifficultyEnum Difficulty { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
    }
}

