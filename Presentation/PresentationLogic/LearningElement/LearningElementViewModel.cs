using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement;

public class LearningElementViewModel : ISerializableViewModel, ILearningElementViewModel
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected LearningElementViewModel()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Parent = null;
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - n.stich
        LearningContent = null!;
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.None;
        Workload = 0;
        Points = 0;
        PositionX = 0;
        PositionY = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningElementViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning element.</param>
    /// <param name="shortname">The short name (abbreviation) of the learning element (Maybe not relevant).</param>
    /// <param name="learningContent">Represents the loaded content of the learning element.</param>
    /// <param name="authors">The string containing the names of all the authors working on the learning element.</param>
    /// <param name="description">A description of the learning element and its contents.</param>
    /// <param name="goals">A description of the goals this learning element is supposed to achieve.</param>
    /// <param name="difficulty">Difficulty of the learning element.</param>
    /// <param name="parent">Decides whether the learning element belongs to a learning world or a learning space.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="points">The number of points of the learning element.</param>
    /// <param name="positionX">x-position of the learning element in the workspace.</param>
    /// <param name="positionY">y-position of the learning element in the workspace.</param>
    public LearningElementViewModel(string name, string shortname,
        ILearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, ILearningSpaceViewModel? parent = null,
        int workload = 0, int points = 0, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Parent = parent;
        LearningContent = learningContent;
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        Points = points;
        PositionX = positionX;
        PositionY = positionY;
    }
    
    public const string fileEnding = "aef";
    public string FileEnding => fileEnding;
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public ILearningSpaceViewModel? Parent { get; set; }
    public ILearningContentViewModel LearningContent { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public int Workload { get; set; }
    public int Points { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}