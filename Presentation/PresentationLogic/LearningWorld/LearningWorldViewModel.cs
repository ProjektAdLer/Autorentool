using System.ComponentModel;
using System.Runtime.CompilerServices;
using AutoMapper.Configuration.Annotations;
using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public class LearningWorldViewModel : ILearningWorldViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private LearningWorldViewModel()
    {
        Id = Guid.Empty;
        _name = "";
        _shortname = "";
        _authors = "";
        _language = "";
        _description = "";
        _goals = "";
        _unsavedChanges = false;
        _learningSpaces = new List<ILearningSpaceViewModel>();
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningWorldViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning world.</param>
    /// <param name="shortname">The short name (abbreviation) of the learning world.</param>
    /// <param name="authors">The string containing the names of all the authors working on the learning world.</param>
    /// <param name="language">The primary language used in this learning world.</param>
    /// <param name="description">A description of the learning world and its contents.</param>
    /// <param name="goals">A description of the goals this learning world is supposed to achieve.</param>
    /// <param name="unsavedChanges">Whether or not the object contains changes that are yet to be saved to disk.</param>
    /// <param name="learningSpaces">Optional collection of learning spaces contained in the learning world.
    /// Should be used when loading a saved learnign world into the application.</param>
    public LearningWorldViewModel(string name, string shortname, string authors, string language, string description,
        string goals, bool unsavedChanges = true, List<ILearningSpaceViewModel>? learningSpaces = null)
    {
        Id = Guid.NewGuid();
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _language = language;
        _description = description;
        _goals = goals;
        _unsavedChanges = unsavedChanges;
        _learningSpaces = learningSpaces ?? new List<ILearningSpaceViewModel>();
    }
    
    public const string fileEnding = "awf";
    
    public Guid Id { get; private set; }
    private ICollection<ILearningSpaceViewModel> _learningSpaces;
    private string _name;
    private string _shortname;
    private string _authors;
    private string _language;
    private string _description;
    private string _goals;
    private bool _unsavedChanges;
    private ILearningSpaceViewModel? _selectedLearningSpace;
    private bool _showingLearningSpaceView;

    public string FileEnding => fileEnding;

    public ICollection<ILearningSpaceViewModel> LearningSpaces
    {
        get => _learningSpaces;
        set
        {
            if (!SetField(ref _learningSpaces, value)) return;
            OnPropertyChanged(nameof(Workload));
        }
    }
    public int Workload =>
        LearningSpaces.Sum(space => space.Workload);

    public int Points =>
        LearningSpaces.Sum(space => space.Points);
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

    public ILearningSpaceViewModel? SelectedLearningSpace
    {
        get => _selectedLearningSpace;
        set => SetField(ref _selectedLearningSpace, value);
    }

    public bool ShowingLearningSpaceView
    {
        get => _showingLearningSpaceView;
        //TODO: Throw exception if ShowingLearningSpaceView is set to true but LearningSpaceViewModel in LearningSpacePresenter is null
        set => SetField(ref _showingLearningSpaceView, value);
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