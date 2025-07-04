using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Theme;

namespace Presentation.PresentationLogic.LearningSpace;

public class LearningSpaceViewModel : ISerializableViewModel, ILearningSpaceViewModel
{
    public const string fileEnding = "asf";
    private string _description;

    private string _name;
    private double _positionX;
    private double _positionY;
    private int _requiredPoints;
    private SpaceTheme _spaceTheme;

    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private LearningSpaceViewModel()
    {
        InitializeFields();
        Id = Guid.Empty;
        Name = "";
        Description = "";
        LearningOutcomeCollection = new LearningOutcomeCollectionViewModel();
        RequiredPoints = 0;
        LearningSpaceLayout = new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X30_8L);
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
    /// <param name="spaceTheme">The theme of the learning space.</param>
    /// <param name="requiredPoints">Points required to complete the learning space.</param>
    /// <param name="learningOutcomes">The learning outcomes of the learning space.</param>
    /// <param name="layoutViewModel">Layout of the learning space</param>
    /// <param name="positionX">x-position of the learning space in the workspace.</param>
    /// <param name="positionY">y-position of the learning space in the workspace.</param>
    /// <param name="inBoundObjects">A List of objects that have learning path to the space.</param>
    /// <param name="outBoundObjects">A list of objects that this space have a learning path to.</param>
    /// <param name="assignedTopic">Topic to which the learning space is assigned.</param>
    public LearningSpaceViewModel(string name, string description, SpaceTheme spaceTheme,
        int requiredPoints = 0, LearningOutcomeCollectionViewModel? learningOutcomes = null,
        ILearningSpaceLayoutViewModel? layoutViewModel = null, double positionX = 0, double positionY = 0,
        ICollection<IObjectInPathWayViewModel>? inBoundObjects = null,
        ICollection<IObjectInPathWayViewModel>? outBoundObjects = null,
        TopicViewModel? assignedTopic = null)
    {
        InitializeFields();
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        LearningOutcomeCollection = learningOutcomes ?? new LearningOutcomeCollectionViewModel();
        SpaceTheme = spaceTheme;
        RequiredPoints = requiredPoints;
        LearningSpaceLayout = layoutViewModel ?? new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X20_6L);
        InBoundObjects = inBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        OutBoundObjects = outBoundObjects ?? new Collection<IObjectInPathWayViewModel>();
        AssignedTopic = assignedTopic;
        PositionX = positionX;
        PositionY = positionY;
    }

    public ILearningSpaceLayoutViewModel LearningSpaceLayout { get; set; }
    public ICollection<IObjectInPathWayViewModel> InBoundObjects { get; set; }
    public ICollection<IObjectInPathWayViewModel> OutBoundObjects { get; set; }
    public TopicViewModel? AssignedTopic { get; set; }
    public LearningOutcomeCollectionViewModel LearningOutcomeCollection { get; set; }
    public int Workload => ContainedLearningElements.Sum(element => element.Workload);

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public int Points => ContainedLearningElements.Sum(element => element.Points);
    
    public int NumberOfElements => ContainedLearningElements.Count();

    public int NumberOfRequiredElements => ContainedLearningElements.Count(element => element.IsRequired);

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

    public int RequiredPoints
    {
        get => _requiredPoints;
        set => SetField(ref _requiredPoints, value);
    }

    public SpaceTheme SpaceTheme
    {
        get => _spaceTheme;
        set => SetField(ref _spaceTheme, value);
    }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges ||
               ContainedLearningElements.Any(element => element.UnsavedChanges) ||
               LearningSpaceLayout.StoryElements.Values.Any(element => element.UnsavedChanges) ||
               LearningOutcomeCollection.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

    public double PositionX
    {
        get => _positionX;
        set
        {
            _positionX = value switch
            {
                < 0 => 0,
                _ => value
            };
        }
    }

    public double PositionY
    {
        get => _positionY;
        set
        {
            _positionY = value switch
            {
                < 0 => 0,
                _ => value
            };
        }
    }

    public double InputConnectionX => PositionX + 40;
    public double InputConnectionY => PositionY - 7;
    public double OutputConnectionX => PositionX + 40;
    public double OutputConnectionY => PositionY + 78;

    public IEnumerable<ILearningElementViewModel> ContainedLearningElements =>
        LearningSpaceLayout.ContainedLearningElements;

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }


    public event PropertyChangedEventHandler? PropertyChanged;
    public string FileEnding => fileEnding;

    [MemberNotNull(nameof(_name), nameof(_description))]
    private void InitializeFields()
    {
        _name = "";
        _description = "";
    }

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