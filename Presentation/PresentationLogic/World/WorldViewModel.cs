using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;

namespace Presentation.PresentationLogic.World;

public class WorldViewModel : IWorldViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private WorldViewModel()
    {
        Id = Guid.Empty;
        _name = "";
        _shortname = "";
        _authors = "";
        _language = "";
        _description = "";
        _goals = "";
        _unsavedChanges = false;
        _spaces = new List<ISpaceViewModel>();
        _pathWayConditions = new List<PathWayConditionViewModel>();
        _pathWays = new List<IPathWayViewModel>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the  world.</param>
    /// <param name="shortname">The short name (abbreviation) of the  world.</param>
    /// <param name="authors">The string containing the names of all the authors working on the  world.</param>
    /// <param name="language">The primary language used in this  world.</param>
    /// <param name="description">A description of the  world and its contents.</param>
    /// <param name="goals">A description of the goals this  world is supposed to achieve.</param>
    /// <param name="unsavedChanges">Whether or not the object contains changes that are yet to be saved to disk.</param>
    /// <param name="spaces">Optional collection of  spaces contained in the  world.
    /// Should be used when loading a saved learnign world into the application.</param>
    /// <param name="pathWayConditions">Conditions within  pathways.</param>
    /// <param name="pathWays">Optional collection of  pathways in the  world.</param>
    /// <param name="unplacedElements">All  elements in the  world that are not placed in any  space</param>
    public WorldViewModel(string name, string shortname, string authors, string language, string description,
        string goals, bool unsavedChanges = true, List<ISpaceViewModel>? spaces = null,
        List<PathWayConditionViewModel>? pathWayConditions = null, List<IPathWayViewModel>? pathWays = null,
        List<IElementViewModel>? unplacedElements = null)
    {
        Id = Guid.NewGuid();
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _language = language;
        _description = description;
        _goals = goals;
        _unsavedChanges = unsavedChanges;
        _spaces = spaces ?? new List<ISpaceViewModel>();
        _pathWayConditions = pathWayConditions ?? new List<PathWayConditionViewModel>();
        _pathWays = pathWays ?? new List<IPathWayViewModel>();
        _unplacedElements = unplacedElements ?? new List<IElementViewModel>();
    }
    
    public const string fileEnding = "awf";
    
    public Guid Id { get; private set; }
    private ICollection<ISpaceViewModel> _spaces;
    private ICollection<PathWayConditionViewModel> _pathWayConditions;
    private ICollection<IPathWayViewModel> _pathWays;
    private ICollection<IElementViewModel> _unplacedElements;
    private string _name;
    private string _shortname;
    private string _authors;
    private string _language;
    private string _description;
    private string _goals;
    private bool _unsavedChanges;
    private ISelectableObjectInWorldViewModel? _selectedObject;
    private IObjectInPathWayViewModel? _onHoveredObject;
    private bool _showingSpaceView;

    public string FileEnding => fileEnding;

    public ICollection<ISpaceViewModel> Spaces
    {
        get => _spaces;
        set
        {
            if (!SetField(ref _spaces, value)) return;
            OnPropertyChanged(nameof(Workload));
        }
    }
    
    public ICollection<PathWayConditionViewModel> PathWayConditions
    {
        get => _pathWayConditions;
        set => SetField(ref _pathWayConditions, value);
    }

    public ICollection<IPathWayViewModel> PathWays
    {
        get => _pathWays;
        set => SetField(ref _pathWays, value);
    }
    
    public ICollection<IElementViewModel> UnplacedElements
    {
        get => _unplacedElements;
        set => SetField(ref _unplacedElements, value);
    } 

    public IEnumerable<IObjectInPathWayViewModel> ObjectsInPathWays =>
        Spaces.Concat<IObjectInPathWayViewModel>(PathWayConditions);
    
    public IEnumerable<ISelectableObjectInWorldViewModel> SelectableWorldObjects =>
        ObjectsInPathWays.Concat<ISelectableObjectInWorldViewModel>(PathWays);

    public int Workload =>
        Spaces.Sum(space => space.Workload);

    public int Points =>
        Spaces.Sum(space => space.Points);
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Shortname
    {
        get => _shortname;
        set => SetField(ref _shortname, value);
    }

    public string Authors
    {
        get => _authors;
        set => SetField(ref _authors, value);
    }

    public string Language
    {
        get => _language;
        set => SetField(ref _language, value);
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

    public bool UnsavedChanges
    {
        get => _unsavedChanges;
        set => SetField(ref _unsavedChanges, value);
    }

    public ISelectableObjectInWorldViewModel? SelectedObject
    {
        get => _selectedObject;
        set => SetField(ref _selectedObject, value);
    }
    
    public IObjectInPathWayViewModel? OnHoveredObjectInPathWay
    {
        get => _onHoveredObject;
        set => SetField(ref _onHoveredObject, value);
    }

    public bool ShowingSpaceView
    {
        get => _showingSpaceView;
        //TODO: Throw exception if ShowingSpaceView is set to true but SpaceViewModel in SpacePresenter is null
        set => SetField(ref _showingSpaceView, value);
    }
    

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}