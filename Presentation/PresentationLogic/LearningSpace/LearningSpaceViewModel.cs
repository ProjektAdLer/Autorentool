using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.Topic;
using Shared;

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
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        LearningSpaceLayout = new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3);
        InBoundObjects = new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = new Collection<IObjectInPathWayViewModel>();
        PositionX = 0;
        PositionY = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning space.</param>
    /// <param name="description">A description of the learning space and its contents.</param>
    /// <param name="goals">A description of the goals this learning space is supposed to achieve.</param>
    /// <param name="requiredPoints">Points required to complete the learning space.</param>
    /// <param name="layoutViewModel">Layout of the learning space</param>
    /// <param name="positionX">x-position of the learning space in the workspace.</param>
    /// <param name="positionY">y-position of the learning space in the workspace.</param>
    /// <param name="inBoundObjects">A List of objects that have learning path to the space.</param>
    /// <param name="outBoundObjects">A list of objects that this space have a learning path to.</param>
    /// <param name="assignedTopic">Topic to which the learning space is assigned.</param>
    public LearningSpaceViewModel(string name, string description, string goals,
        int requiredPoints = 0,
        ILearningSpaceLayoutViewModel? layoutViewModel = null, double positionX = 0, double positionY = 0,
        ICollection<IObjectInPathWayViewModel>? inBoundObjects = null,
        ICollection<IObjectInPathWayViewModel>? outBoundObjects = null,
        TopicViewModel? assignedTopic = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        LearningSpaceLayout = layoutViewModel ?? new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X2);
        InBoundObjects = inBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = outBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        AssignedTopic = assignedTopic;
        PositionX = positionX;
        PositionY = positionY;
    }

    public const string fileEnding = "asf";
    public string FileEnding => fileEnding;
    public ILearningSpaceLayoutViewModel LearningSpaceLayout { get; set; }
    public ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    public ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
    public TopicViewModel? AssignedTopic { get; set; }
    public int Workload => ContainedLearningElements.Sum(element => element.Workload);
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public int Points => ContainedLearningElements.Sum(element => element.Points);
    
    public Guid Id { get; private set; }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public string Goals
    {
        get => _goals;
        set => SetField(ref _goals, value);
    }

    public int RequiredPoints
    {
        get => _requiredPoints;
        set => SetField(ref _requiredPoints, value);
    }

    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public double InputConnectionX => PositionX + 42;
    public double InputConnectionY => PositionY - 7;
    public double OutputConnectionX => PositionX + 42;
    public double OutputConnectionY => PositionY + 82;

    private ILearningElementViewModel? _selectedLearningElement;

    private string _name;
    private string _description;
    private string _goals;
    private int _requiredPoints;
    public event EventHandler<EventArgs>? SelectedElementChanged;

    public ILearningElementViewModel? SelectedLearningElement
    {
        get { return _selectedLearningElement; }
        set
        {
            _selectedLearningElement = value;
            OnSelectedElementChanged();
        }
    }

    public IEnumerable<ILearningElementViewModel> ContainedLearningElements =>
        LearningSpaceLayout.ContainedLearningElements;

    protected virtual void OnSelectedElementChanged()
    {
        SelectedElementChanged?.Invoke(this, EventArgs.Empty);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}