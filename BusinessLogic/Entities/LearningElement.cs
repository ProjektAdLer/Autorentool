using BusinessLogic.Entities.LearningContent;
using JetBrains.Annotations;
using Shared;
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
        Id = Guid.NewGuid();
        Name = "";
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - n.stich
        LearningContent = null!;
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.None;
        ElementModel = ElementModel.l_h5p_slotmachine_1;
        Workload = 0;
        Points = 1;
        UnsavedChanges = false;
        PositionX = 0;
        PositionY = 0;
        Parent = null;
    }

    public LearningElement(string name, ILearningContent learningContent,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        ILearningSpace? parent = null, int workload = 0, int points = 0,
        double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        LearningContent = learningContent;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        ElementModel = elementModel;
        Workload = workload;
        Points = points;
        UnsavedChanges = true;
        PositionX = positionX;
        PositionY = positionY;
        Parent = parent;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public ILearningSpace? Parent { get; set; }
    public ILearningContent LearningContent { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public ElementModel ElementModel { get; set; }
    public int Workload { get; set; }
    public int Points { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || LearningContent.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

    public LearningElementDifficultyEnum Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }

    public IMemento GetMemento()
    {
        return new LearningElementMemento(Name, LearningContent, Description, Goals, Workload,
            Points, Difficulty, ElementModel, Parent, PositionX, PositionY, UnsavedChanges);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningElementMemento learningElementMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = learningElementMemento.Name;
        LearningContent = learningElementMemento.Content;
        Description = learningElementMemento.Description;
        Goals = learningElementMemento.Goals;
        Workload = learningElementMemento.Workload;
        Points = learningElementMemento.Points;
        Difficulty = learningElementMemento.Difficulty;
        ElementModel = learningElementMemento.ElementModel;
        Parent = learningElementMemento.Parent;
        PositionX = learningElementMemento.PositionX;
        PositionY = learningElementMemento.PositionY;
        UnsavedChanges = learningElementMemento.UnsavedChanges;
    }

    private record LearningElementMemento : IMemento
    {
        internal LearningElementMemento(string name, ILearningContent content, string description, string goals,
            int workload, int points, LearningElementDifficultyEnum difficulty, ElementModel elementModel, ILearningSpace? parent,
            double positionX, double positionY, bool unsavedChanges)
        {
            Name = name;
            Content = content;
            Description = description;
            Goals = goals;
            Workload = workload;
            Points = points;
            Difficulty = difficulty;
            ElementModel = elementModel;
            Parent = parent;
            PositionX = positionX;
            PositionY = positionY;
            UnsavedChanges = unsavedChanges;
        }
        
        internal string Name { get; }
        internal ILearningSpace? Parent { get; }
        internal ILearningContent Content { get; }
        internal string Description { get; }
        internal string Goals { get; }
        internal int Workload { get; }
        internal int Points { get; }
        internal LearningElementDifficultyEnum Difficulty { get; }
        internal ElementModel ElementModel { get; }
        internal double PositionX { get; }
        internal double PositionY { get; }
        public bool UnsavedChanges { get; }
    }
}

