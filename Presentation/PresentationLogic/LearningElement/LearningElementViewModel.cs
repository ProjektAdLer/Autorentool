using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement;

public class LearningElementViewModel : ISerializableViewModel, ILearningElementViewModel
{
    public const string fileEnding = "aef";
    private string _description;
    private LearningElementDifficultyEnum _difficulty;
    private ElementModel _elementModel;
    private string _goals;
    private Guid _id;
    private bool _internalUnsavedChanges;
    private ILearningContentViewModel _learningContent;
    private string _name;
    private ILearningSpaceViewModel? _parent;
    private int _points;
    private double _positionX;
    private double _positionY;
    private int _workload;

    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected LearningElementViewModel()
    {
        _id = Guid.Empty;
        _name = "";
        _parent = null;
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - n.stich
        _learningContent = null!;
        _description = "";
        _goals = "";
        _difficulty = LearningElementDifficultyEnum.None;
        _elementModel = ElementModel.l_h5p_slotmachine_1;
        _workload = 0;
        _points = 1;
        _positionX = 0;
        _positionY = 0;
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
        _id = Guid.NewGuid();
        _name = name;
        _parent = parent;
        _learningContent = learningContent;
        _description = description;
        _goals = goals;
        _difficulty = difficulty;
        _elementModel = elementModel;
        _workload = workload;
        _points = points;
        _positionX = positionX;
        _positionY = positionY;
        UnsavedChanges = true;
    }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local - required for automapper n.stich
    public Guid Id
    {
        get => _id;
        private set => SetField(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public ILearningSpaceViewModel? Parent
    {
        get => _parent;
        set => SetField(ref _parent, value);
    }

    public ILearningContentViewModel LearningContent
    {
        get => _learningContent;
        set => SetField(ref _learningContent, value);
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

    public LearningElementDifficultyEnum Difficulty
    {
        get => _difficulty;
        set => SetField(ref _difficulty, value);
    }

    public ElementModel ElementModel
    {
        get => _elementModel;
        set => SetField(ref _elementModel, value);
    }

    public int Workload
    {
        get => _workload;
        set => SetField(ref _workload, value);
    }

    public int Points
    {
        get => _points;
        set => SetField(ref _points, value);
    }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges
    {
        get => _internalUnsavedChanges;
        private set => SetField(ref _internalUnsavedChanges, value);
    }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || LearningContent.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

    public double PositionX
    {
        get => _positionX;
        set => SetField(ref _positionX, value);
    }

    public double PositionY
    {
        get => _positionY;
        set => SetField(ref _positionY, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public string FileEnding => fileEnding;

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