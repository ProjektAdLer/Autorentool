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
        Parent = null;
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - n.stich
        LearningContent = null!;
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.None;
        ElementModel = ElementModel.l_h5p_slotmachine_1;
        Workload = 0;
        Points = 1;
        PositionX = 0;
        PositionY = 0;
        UnsavedChanges = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningElementViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning element.</param>
    /// <param name="learningContent">Represents the loaded content of the learning element.</param>
    /// <param name="description">A description of the learning element and its contents.</param>
    /// <param name="goals">A description of the goals this learning element is supposed to achieve.</param>
    /// <param name="difficulty">Difficulty of the learning element.</param>
    /// <param name="elementModel">Theme of the learning element</param>
    /// <param name="parent">Decides whether the learning element belongs to a learning world or a learning space.</param>
    /// <param name="workload">The time required to complete the learning element.</param>
    /// <param name="points">The number of points of the learning element.</param>
    /// <param name="positionX">x-position of the learning element in the workspace.</param>
    /// <param name="positionY">y-position of the learning element in the workspace.</param>
    public LearningElementViewModel(string name, 
        ILearningContentViewModel learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, ILearningSpaceViewModel? parent = null,
        int workload = 0, int points = 0, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Parent = parent;
        LearningContent = learningContent;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        ElementModel = elementModel;
        Workload = workload;
        Points = points;
        PositionX = positionX;
        PositionY = positionY;
        UnsavedChanges = true;
    }
    
    public const string fileEnding = "aef";
    public string FileEnding => fileEnding;
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public ILearningSpaceViewModel? Parent { get; set; }
    public ILearningContentViewModel LearningContent { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public ElementModel ElementModel { get; set; }
    public int Workload { get; set; }
    public int Points { get; set; }

    private bool InternalUnsavedChanges { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || LearningContent.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

    public double PositionX { get; set; }
    public double PositionY { get; set; }
}