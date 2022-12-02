using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

public class LearningSpaceViewModel : ISerializableViewModel, ILearningSpaceViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private LearningSpaceViewModel()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        LearningElements = new Collection<ILearningElementViewModel>();
        InBoundObjects = new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = new Collection<IObjectInPathWayViewModel>();
        PositionX = 0;
        PositionY = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning space.</param>
    /// <param name="shortname">The short name (abbreviation) of the learning world.(Maybe not relevant)</param>
    /// <param name="authors">The string containing the names of all the authors working on the learning space.</param>
    /// <param name="description">A description of the learning space and its contents.</param>
    /// <param name="goals">A description of the goals this learning space is supposed to achieve.</param>
    /// <param name="requiredPoints">Points required to complete the learning space.</param>
    /// <param name="learningElements">Optional collection of learning elements contained in the learning space.</param>
    /// <param name="inBoundObjects">A List of objects that have learning path to the space.</param>
    /// <param name="outBoundObjects">A list of objects that this space have a learning path to.</param>
    /// <param name="positionX">x-position of the learning space in the workspace.</param>
    /// <param name="positionY">y-position of the learning space in the workspace.</param>
    public LearningSpaceViewModel(string name, string shortname, string authors, string description, string goals, int requiredPoints = 0,
        ICollection<ILearningElementViewModel>? learningElements = null, double positionX = 0, double positionY = 0,
        ICollection<IObjectInPathWayViewModel>? inBoundObjects = null, ICollection<IObjectInPathWayViewModel>? outBoundObjects = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        LearningElements = learningElements ?? new Collection<ILearningElementViewModel>();
        InBoundObjects = inBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = outBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        PositionX = positionX;
        PositionY = positionY;
    }

    public const string fileEnding = "asf";
    public string FileEnding => fileEnding;
    public ICollection<ILearningElementViewModel> LearningElements { get; set; }
    public ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    public ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
    public int Workload => LearningElements.Sum(element => element.Workload);
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public int Points => LearningElements.Sum(element => element.Points);
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int RequiredPoints { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public double InputConnectionX => PositionX + 42;
    public double InputConnectionY => PositionY - 7;
    public double OutputConnectionX => PositionX + 42; 
    public double OutputConnectionY => PositionY + 82;
    public ILearningElementViewModel? SelectedLearningElement { get; set; }
}
