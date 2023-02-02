using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class Element : IElement, IOriginator
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    protected Element()
    {
        Id = Guid.NewGuid();
        Name = "";
        Shortname = "";
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - n.stich
        Content = null!;
        Url = "";
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = ElementDifficultyEnum.None;
        Workload = 0;
        Points = 0;
        PositionX = 0;
        PositionY = 0;
        Parent = null;
    }

    public Element(string name, string shortname, Content content, string url,
        string authors, string description, string goals, ElementDifficultyEnum difficulty,
        ISpace? parent = null, int workload = 0, int points = 0,
        double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Content = content;
        Url = url ?? "";
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        Points = points;
        PositionX = positionX;
        PositionY = positionY;
        Parent = parent;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public ISpace? Parent { get; set; }
    public Content Content { get; set; }
    public string Url { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int Workload { get; set; }
    public int Points { get; set; }
    public ElementDifficultyEnum Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }

    public IMemento GetMemento()
    {
        return new ElementMemento(Name, Shortname, Content, Authors, Description, Goals, Workload,
            Points, Difficulty, Parent, PositionX, PositionY);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not ElementMemento elementMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = elementMemento.Name;
        Shortname = elementMemento.Shortname;
        Content = elementMemento.Content;
        Authors = elementMemento.Authors;
        Description = elementMemento.Description;
        Goals = elementMemento.Goals;
        Workload = elementMemento.Workload;
        Points = elementMemento.Points;
        Difficulty = elementMemento.Difficulty;
        Parent = elementMemento.Parent;
        PositionX = elementMemento.PositionX;
        PositionY = elementMemento.PositionY;
    }

    private record ElementMemento : IMemento
    {
        internal ElementMemento(string name, string shortname, Content content, string authors,
            string description, string goals, int workload, int points, ElementDifficultyEnum difficulty,
            ISpace? parent, double positionX = 0, double positionY = 0)
        {
            Name = name;
            Shortname = shortname;
            Content = content;
            Authors = authors;
            Description = description;
            Goals = goals;
            Workload = workload;
            Points = points;
            Difficulty = difficulty;
            Parent = parent;
            PositionX = positionX;
            PositionY = positionY;
        }
        
        internal string Name { get; }
        internal string Shortname { get; }
        internal ISpace? Parent { get; }
        internal Content Content { get; }
        internal string Authors { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int Workload { get; }
        internal int Points { get; }
        internal ElementDifficultyEnum Difficulty { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
    }
}

