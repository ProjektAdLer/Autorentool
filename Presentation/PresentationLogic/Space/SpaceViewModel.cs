using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space.SpaceLayout;

namespace Presentation.PresentationLogic.Space;

public class SpaceViewModel : ISerializableViewModel, ISpaceViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private SpaceViewModel()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        SpaceLayout = new SpaceLayoutViewModel();
        InBoundObjects = new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = new Collection<IObjectInPathWayViewModel>();
        PositionX = 0;
        PositionY = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the  space.</param>
    /// <param name="shortname">The short name (abbreviation) of the  world.(Maybe not relevant)</param>
    /// <param name="authors">The string containing the names of all the authors working on the  space.</param>
    /// <param name="description">A description of the  space and its contents.</param>
    /// <param name="goals">A description of the goals this  space is supposed to achieve.</param>
    /// <param name="requiredPoints">Points required to complete the  space.</param>
    /// <param name="layoutViewModel">Layout of the  space</param>
    /// <param name="inBoundObjects">A List of objects that have  path to the space.</param>
    /// <param name="outBoundObjects">A list of objects that this space have a  path to.</param>
    /// <param name="positionX">x-position of the  space in the workspace.</param>
    /// <param name="positionY">y-position of the  space in the workspace.</param>
    public SpaceViewModel(string name, string shortname, string authors, string description, string goals, int requiredPoints = 0,
        ISpaceLayoutViewModel? layoutViewModel = null, double positionX = 0, double positionY = 0,
        ICollection<IObjectInPathWayViewModel>? inBoundObjects = null, ICollection<IObjectInPathWayViewModel>? outBoundObjects = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        SpaceLayout = layoutViewModel ?? new SpaceLayoutViewModel();
        InBoundObjects = inBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = outBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        PositionX = positionX;
        PositionY = positionY;
    }

    public const string fileEnding = "asf";
    public string FileEnding => fileEnding;
    public ISpaceLayoutViewModel SpaceLayout { get; set; }
    public ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    public ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
    public int Workload => ContainedElements.Sum(element => element.Workload);
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public int Points => ContainedElements.Sum(element => element.Points);
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
    public IElementViewModel? SelectedElement { get; set; }
    public IEnumerable<IElementViewModel> ContainedElements => SpaceLayout.ContainedElements;
}
