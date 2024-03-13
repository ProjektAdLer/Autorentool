using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningContent.Story;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.Topic;

namespace Presentation.PresentationLogic.LearningWorld;

public class LearningWorldViewModel : ILearningWorldViewModel
{
    public const string fileEnding = "awf";
    private string _authors;
    private string _description;
    private string _enrolmentKey;
    private string _evaluationLink;
    private string _goals;
    private string _language;
    private ICollection<ILearningPathWayViewModel> _learningPathWays;
    private ICollection<ILearningSpaceViewModel> _learningSpaces;
    private string _name;
    private IObjectInPathWayViewModel? _onHoveredLearningObject;
    private ICollection<PathWayConditionViewModel> _pathWayConditions;
    private string _savePath;
    private string _shortname;
    private ICollection<TopicViewModel> _topics;
    private ICollection<ILearningElementViewModel> _unplacedLearningElements;

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
        _evaluationLink = "";
        _enrolmentKey = "";
        _savePath = "";
        InternalUnsavedChanges = false;
        _learningSpaces = new List<ILearningSpaceViewModel>();
        _pathWayConditions = new List<PathWayConditionViewModel>();
        _learningPathWays = new List<ILearningPathWayViewModel>();
        _unplacedLearningElements = new List<ILearningElementViewModel>();
        _topics = new List<TopicViewModel>();
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
    /// <param name="evaluationLink">Link to the evaluation on completion.</param>
    /// <param name="enrolmentKey">Key for users to enrol in the learning world.</param>
    /// <param name="savePath">The save path of the learning world.</param>
    /// <param name="unsavedChanges">Whether or not the object contains changes that are yet to be saved to disk.</param>
    /// <param name="learningSpaces">Optional collection of learning spaces contained in the learning world.
    /// Should be used when loading a saved learnign world into the application.</param>
    /// <param name="pathWayConditions">Conditions within learning pathways.</param>
    /// <param name="learningPathWays">Optional collection of learning pathways in the learning world.</param>
    /// <param name="unplacedLearningElements">All learning elements in the learning world that are not placed in any learning space</param>
    /// <param name="topics">Optional collection of topics in the learning world.</param>
    public LearningWorldViewModel(string name, string shortname, string authors, string language, string description,
        string goals, string evaluationLink, string enrolmentKey, string savePath = "", bool unsavedChanges = true,
        List<ILearningSpaceViewModel>? learningSpaces = null,
        List<PathWayConditionViewModel>? pathWayConditions = null,
        List<ILearningPathWayViewModel>? learningPathWays = null,
        List<ILearningElementViewModel>? unplacedLearningElements = null,
        List<TopicViewModel>? topics = null)
    {
        Id = Guid.NewGuid();
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _language = language;
        _description = description;
        _goals = goals;
        _evaluationLink = evaluationLink;
        _enrolmentKey = enrolmentKey;
        _savePath = savePath;
        InternalUnsavedChanges = unsavedChanges;
        _learningSpaces = learningSpaces ?? new List<ILearningSpaceViewModel>();
        _pathWayConditions = pathWayConditions ?? new List<PathWayConditionViewModel>();
        _learningPathWays = learningPathWays ?? new List<ILearningPathWayViewModel>();
        _unplacedLearningElements = unplacedLearningElements ?? new List<ILearningElementViewModel>();
        _topics = topics ?? new List<TopicViewModel>();
    }

    public IEnumerable<ISelectableObjectInWorldViewModel> SelectableWorldObjects =>
        ObjectsInPathWays.Concat<ISelectableObjectInWorldViewModel>(LearningPathWays);

    public Guid Id { get; private set; }

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

    public ICollection<PathWayConditionViewModel> PathWayConditions
    {
        get => _pathWayConditions;
        set => SetField(ref _pathWayConditions, value);
    }

    public ICollection<ILearningPathWayViewModel> LearningPathWays
    {
        get => _learningPathWays;
        set => SetField(ref _learningPathWays, value);
    }

    public ICollection<ILearningElementViewModel> UnplacedLearningElements
    {
        get => _unplacedLearningElements;
        set => SetField(ref _unplacedLearningElements, value);
    }

    public IEnumerable<ILearningElementViewModel> AllLearningElements =>
        LearningSpaces
            .SelectMany(space => space.ContainedLearningElements)
            .Concat(UnplacedLearningElements.Where(ele => ele.LearningContent is not StoryContentViewModel));

    public IEnumerable<ILearningElementViewModel> AllStoryElements =>
        LearningSpaces
            .SelectMany(space => space.LearningSpaceLayout.StoryElements.Values)
            .Concat(UnplacedLearningElements.Where(ele => ele.LearningContent is StoryContentViewModel));

    public ICollection<TopicViewModel> Topics
    {
        get => _topics;
        set => SetField(ref _topics, value);
    }

    public IEnumerable<IObjectInPathWayViewModel> ObjectsInPathWays =>
        LearningSpaces.Concat<IObjectInPathWayViewModel>(PathWayConditions);

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

    public string EvaluationLink
    {
        get => _evaluationLink;
        set => SetField(ref _evaluationLink, value);
    }

    public string EnrolmentKey
    {
        get => _enrolmentKey;
        set => SetField(ref _enrolmentKey, value);
    }

    public string SavePath
    {
        get => _savePath;
        set => SetField(ref _savePath, value);
    }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges ||
               LearningSpaces.Any(space => space.UnsavedChanges) ||
               UnplacedLearningElements.Any(element => element.UnsavedChanges) ||
               PathWayConditions.Any(condition => condition.UnsavedChanges) ||
               Topics.Any(topic => topic.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }


    public IObjectInPathWayViewModel? OnHoveredObjectInPathWay
    {
        get => _onHoveredLearningObject;
        set => SetField(ref _onHoveredLearningObject, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }


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