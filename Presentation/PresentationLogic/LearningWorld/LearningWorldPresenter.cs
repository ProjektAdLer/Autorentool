using System.ComponentModel;
using System.Runtime.CompilerServices;
using BusinessLogic.Validation;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningWorld;

public class LearningWorldPresenter : ILearningWorldPresenter, ILearningWorldPresenterToolboxInterface,
    ILearningWorldPresenterOverviewInterface
{
    public LearningWorldPresenter(
        IPresentationLogic presentationLogic, ILearningSpacePresenter learningSpacePresenter,
        ILogger<LearningWorldPresenter> logger, IMediator mediator,
        ISelectedViewModelsProvider selectedViewModelsProvider, IErrorService errorService)
    {
        _learningSpacePresenter = learningSpacePresenter;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _mediator = mediator;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        _selectedViewModelsProvider.PropertyChanged += OnSelectedViewModelsProviderOnPropertyChanged;
        //first-time update in case a learning world was selected before we were instantiated 
        LearningWorldVm = selectedViewModelsProvider.LearningWorld;
        _errorService = errorService;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILogger<LearningWorldPresenter> _logger;
    private readonly IMediator _mediator;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;
    private readonly IErrorService _errorService;

    private ILearningWorldViewModel? _learningWorldVm;
    private IObjectInPathWayViewModel? _newConditionSourceObject;
    private LearningSpaceViewModel? _newConditionTargetSpace;
    private int _spaceCreationCounter = 0;
    private int _conditionCreationCounter = 0;

    public bool SelectedLearningObjectIsSpace =>
        _selectedViewModelsProvider.LearningObjectInPathWay?.GetType() == typeof(LearningSpaceViewModel);

    public bool ShowingLearningSpaceView => LearningWorldVm is { ShowingLearningSpaceView: true };

    /// <summary>
    /// The currently selected LearningWorldViewModel.
    /// </summary>
    public ILearningWorldViewModel? LearningWorldVm
    {
        get => _learningWorldVm;
        internal set
        {
            var selectedLearningObjectIsSpaceBefore = SelectedLearningObjectIsSpace;
            var showingLearningSpaceViewBefore = ShowingLearningSpaceView;
            if (!BeforeSetField(_learningWorldVm, value))
                return;
            SetField(ref _learningWorldVm, value);
            if (SelectedLearningObjectIsSpace != selectedLearningObjectIsSpaceBefore)
                OnPropertyChanged(nameof(SelectedLearningObjectIsSpace));
            if (ShowingLearningSpaceView != showingLearningSpaceViewBefore)
                OnPropertyChanged(nameof(ShowingLearningSpaceView));
            HideRightClickMenu();
        }
    }

    /// <inheritdoc cref="ILearningSpaceNamesProvider.SpaceNames"/>
    public IEnumerable<(Guid, string)>? SpaceNames =>
        LearningWorldVm?.LearningSpaces.Select(space => (space.Id, space.Name));

    public event EventHandler<CommandUndoRedoOrExecuteArgs> OnCommandUndoRedoOrExecute
    {
        add => _presentationLogic.OnCommandUndoRedoOrExecute += value;
        remove => _presentationLogic.OnCommandUndoRedoOrExecute -= value;
    }

    /// <summary>
    /// If any object in the LearningWorld has an active RightClickMenu, this object is set in this variable.
    /// Otherwise, it is null.
    /// </summary>
    public IObjectInPathWayViewModel? RightClickedLearningObject { get; private set; }

    public void OnSelectedViewModelsProviderOnPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_selectedViewModelsProvider.LearningWorld))
        {
            if (caller is not ISelectedViewModelsProvider)
                throw new ArgumentException("Caller must be of type ISelectedViewModelsProvider");

            LearningWorldVm = _selectedViewModelsProvider.LearningWorld;
        }
    }

    #region LearningWorld

    public void EditLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");

        _presentationLogic.EditLearningWorld(LearningWorldVm, name, shortname, authors, language, description, goals);
    }

    /// <summary>
    /// Calls the respective Save methode for Learning Space or Learning Element depending on which learning object is selected
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public async Task SaveLearningWorldAsync()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");

        await _presentationLogic.SaveLearningWorldAsync((LearningWorldViewModel)LearningWorldVm);
    }

    #endregion

    #region ObjectInPathWay

    /// <summary>
    /// Changes the selected <see cref="IObjectInPathWayViewModel"/> in the currently selected learning world.
    /// </summary>
    /// <param name="pathWayObject">The pathway object that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    internal void SetSelectedLearningObject(ISelectableObjectInWorldViewModel pathWayObject)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (SelectedLearningObjectIsSpace)
        {
            _learningSpacePresenter.SetLearningSpace(
                (LearningSpaceViewModel)_selectedViewModelsProvider.LearningObjectInPathWay!);
            _selectedViewModelsProvider.SetLearningObjectInPathWay(pathWayObject, null);
        }
        else
        {
            _selectedViewModelsProvider.SetLearningObjectInPathWay(pathWayObject, null);
        }

        HideRightClickMenu();
    }

    public void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> args)
    {
        _presentationLogic.DragObjectInPathWay(args.LearningObject, args.OldPositionX, args.OldPositionY);
        HideRightClickMenu();
    }

    public void RightClickOnObjectInPathWay(IObjectInPathWayViewModel obj)
    {
        RightClickedLearningObject = obj;
    }

    public void HideRightClickMenu()
    {
        RightClickedLearningObject = null;
    }

    public void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj)
    {
        SetSelectedLearningObject(obj);
    }

    public void DoubleClickOnObjectInPathway(IObjectInPathWayViewModel obj)
    {
        SetSelectedLearningObject(obj);
        if (obj is PathWayConditionViewModel pathwayCondition)
            SwitchPathWayCondition(pathwayCondition);
        ShowSelectedLearningSpaceView();
    }

    public void DeleteLearningObject(IObjectInPathWayViewModel obj)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        switch (obj)
        {
            case PathWayConditionViewModel pathWayConditionViewModel:
                _presentationLogic.DeletePathWayCondition(LearningWorldVm, pathWayConditionViewModel);
                break;
            case LearningSpaceViewModel learningSpaceViewModel:
                _presentationLogic.DeleteLearningSpace(LearningWorldVm, learningSpaceViewModel);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj));
        }
    }

    /// <summary>
    /// Deletes the selected learning object in the currently selected learning world and sets an other space or element as selected learning object.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected learning object is of an other type than space or element.</exception>
    public void DeleteSelectedLearningObject()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        switch (_selectedViewModelsProvider.LearningObjectInPathWay)
        {
            case null:
                return;
            case LearningSpaceViewModel space:
                _presentationLogic.DeleteLearningSpace(LearningWorldVm, space);
                break;
            case LearningPathwayViewModel pathWay:
                _presentationLogic.DeleteLearningPathWay(LearningWorldVm, pathWay);
                break;
            case PathWayConditionViewModel condition:
                _presentationLogic.DeletePathWayCondition(LearningWorldVm, condition);
                break;
        }
    }

    #region LearningSpace

    public void SetSelectedLearningSpace(IObjectInPathWayViewModel obj)
    {
        SetSelectedLearningObject(obj);
        _selectedViewModelsProvider.SetLearningElement(null, null);
        _mediator.RequestOpenSpaceDialog();
    }

    public void ShowSelectedLearningSpaceView()
    {
        if (LearningWorldVm != null) LearningWorldVm.ShowingLearningSpaceView = true;
        HideRightClickMenu();
    }

    public void CloseLearningSpaceView()
    {
        if (LearningWorldVm != null) LearningWorldVm.ShowingLearningSpaceView = false;
    }

    /// <inheritdoc cref="ILearningWorldPresenter.CreateLearningSpace"/>
    public void CreateLearningSpace(string name, string description, string goals, int requiredPoints,
        Theme theme,
        double positionX = 0D,
        double positionY = 0D,
        TopicViewModel? topic = null)
    {
        if (LearningWorldVm == null)
        {
            _errorService.SetError("Error while creating learning space; No Learning World selected");
            return;
        }

        _presentationLogic.CreateLearningSpace(LearningWorldVm, name, description, goals,
            requiredPoints, theme, positionX, positionY, topic);
        //TODO: Return error in the command in case of failure
    }

    /// <inheritdoc cref="ILearningWorldPresenterToolboxInterface.AddLearningSpace"/>
    public void AddLearningSpace(ILearningSpaceViewModel learningSpace)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.AddLearningSpace(LearningWorldVm, learningSpace);
    }

    public void AddNewLearningSpace()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _selectedViewModelsProvider.SetLearningObjectInPathWay(null, null);
        _mediator.RequestOpenSpaceDialog();
    }

    /// <summary>
    /// Calls the LoadLearningSpaceAsync methode in <see cref="_presentationLogic"/> and adds the returned learning space to the current learning world.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="LearningWorldVm"/> is null</exception>
    public async Task LoadLearningSpaceAsync()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        await _presentationLogic.LoadLearningSpaceAsync(LearningWorldVm);
    }

    /// <summary>
    /// Calls the respective Save methode for Learning Space or Learning Element depending on which learning object is selected
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public async Task SaveSelectedLearningSpaceAsync()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (_selectedViewModelsProvider.LearningObjectInPathWay == null)
            throw new ApplicationException("SelectedLearningSpace is null");

        await _presentationLogic.SaveLearningSpaceAsync(
            (LearningSpaceViewModel)_selectedViewModelsProvider.LearningObjectInPathWay);
    }

    public void EditSelectedLearningSpace()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (_selectedViewModelsProvider.LearningObjectInPathWay is not LearningSpaceViewModel)
            throw new ApplicationException("SelectedLearningObjectInPathWay is not LearningSpaceViewModel");
        _mediator.RequestOpenSpaceDialog();
    }

    public void DeleteLearningSpace(ILearningSpaceViewModel obj)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.DeleteLearningSpace(LearningWorldVm, obj);
    }

    #endregion

    #region PathWayCondition

    public void CreatePathWayCondition(ConditionEnum condition = ConditionEnum.Or)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.CreatePathWayCondition(LearningWorldVm, condition, 0, 0);
    }

    public void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition)
    {
        _presentationLogic.EditPathWayCondition(pathWayCondition,
            pathWayCondition.Condition == ConditionEnum.And ? ConditionEnum.Or : ConditionEnum.And);
    }

    public void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.DeletePathWayCondition(LearningWorldVm, pathWayCondition);
    }

    #endregion

    #region LearningPathWay

    /// <summary>
    /// Sets the on hovered learning object of the learning world to the target object on the given position.
    /// When there is no learning object on the given position, the on hovered learning object is set to null.
    /// </summary>
    /// <param name="sourceObject">The learning object from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target object.</param>
    /// <param name="y">The y-coordinate of the target object.</param>
    public void SetOnHoveredObjectInPathWay(IObjectInPathWayViewModel sourceObject, double x, double y)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var objectAtPosition = GetObjectAtPosition(x, y);
        if (objectAtPosition == null || objectAtPosition == sourceObject)
        {
            LearningWorldVm.OnHoveredObjectInPathWay = null;
        }
        else
        {
            LearningWorldVm.OnHoveredObjectInPathWay = objectAtPosition;
            _logger.LogDebug("ObjectAtPosition: {0} ", sourceObject.Id);
        }
    }

    /// <summary>
    /// Localizes and returns the learning object at the given position in the currently selected learning world.
    /// </summary>
    /// <param name="x">The x-coordinate of the target object</param>
    /// <param name="y">The y-coordinate of the target object</param>
    /// <returns>The pathway object at the given position.</returns>
    private IObjectInPathWayViewModel? GetObjectAtPosition(double x, double y)
    {
        //LearningWorldVm can not be null because it is checked before call. -m.ho
        var objectAtPosition = LearningWorldVm?.LearningSpaces.FirstOrDefault(ls =>
                                   ls.PositionX <= x && ls.PositionX + 84 >= x && ls.PositionY <= y &&
                                   ls.PositionY + 84 >= y) ??
                               (IObjectInPathWayViewModel?)LearningWorldVm?.PathWayConditions.FirstOrDefault(lc =>
                                   lc.PositionX <= x && lc.PositionX + 76 >= x && lc.PositionY <= y &&
                                   lc.PositionY + 43 >= y);
        return objectAtPosition;
    }

    /// <summary>
    /// Creates a learning pathway from the given source space to the target space on the given position.
    /// Does nothing when there is no learning space on the given position.
    /// </summary>
    /// <param name="sourceObject">The learning space from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target space</param>
    /// <param name="y">The y-coordinate of the target space</param>
    public void CreateLearningPathWay(IObjectInPathWayViewModel sourceObject, double x, double y)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var targetObject = GetObjectAtPosition(x, y);
        if (targetObject == null || targetObject == sourceObject)
            return;
        LearningWorldVm.OnHoveredObjectInPathWay = null;
        if (targetObject.InBoundObjects.Count == 1 && targetObject is LearningSpaceViewModel space)
        {
            _newConditionSourceObject = sourceObject;
            _newConditionTargetSpace = space;
            return;
        }

        _presentationLogic.CreateLearningPathWay(LearningWorldVm, sourceObject, targetObject);
    }

    /// <summary>
    /// Deletes the last created learning pathway leading to the target space.
    /// </summary>
    /// <param name="targetObject">The learning space where the path ends.</param>
    public void DeleteLearningPathWay(IObjectInPathWayViewModel targetObject)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var learningPathWay = LearningWorldVm.LearningPathWays.LastOrDefault(lp => lp.TargetObject == targetObject);
        if (learningPathWay == null)
            throw new ApplicationException("LearningPathWay is null");
        _presentationLogic.DeleteLearningPathWay(LearningWorldVm, learningPathWay);
    }

    #endregion

    #endregion

    #region LearningElement

    /// <summary>
    /// Sets the selected learning element of the learning world to the given learning element.
    /// </summary>
    /// <param name="learningElement">The learning element to set.</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void SetSelectedLearningElement(ILearningElementViewModel learningElement)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (learningElement.Parent != null)
            _selectedViewModelsProvider.SetLearningObjectInPathWay(learningElement.Parent, null);
        _selectedViewModelsProvider.SetLearningElement(learningElement, null);
        _mediator.RequestOpenElementDialog();
    }

    /// <summary>
    /// Edits a given learning element.
    /// </summary>
    /// <param name="elementParent">The parent of the learning element which is either a learning space or null.</param>
    /// <param name="learningElement">The learning element to edit.</param>
    /// <param name="name">The new name of the element.</param>
    /// <param name="description">The new description of the element.</param>
    /// <param name="goals">The new goals of the element.</param>
    /// <param name="difficulty">The new difficulty of the element.</param>
    /// <param name="workload">The new workload of the element.</param>
    /// <param name="points">The new points of the element.</param>
    /// <param name="learningContent">The new learning content of the element.</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected or the learning
    /// element to edit is not a unplaced element in the learning world.</exception>
    public void EditLearningElement(ILearningSpaceViewModel? elementParent, ILearningElementViewModel learningElement,
        string name, string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, ILearningContentViewModel learningContent)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (LearningWorldVm.UnplacedLearningElements.Contains(learningElement))
            if (learningElement.Parent == null)
                _presentationLogic.EditLearningElement(elementParent, learningElement, name,
                    description,
                    goals, difficulty, workload, points, learningContent);
            else
            {
                throw new ApplicationException("LearningElement is unplaced but has a space parent");
            }
        else
        {
            if (learningElement.Parent == _selectedViewModelsProvider.LearningObjectInPathWay)
                _learningSpacePresenter.EditLearningElement(learningElement, name, description, goals, difficulty,
                    workload, points, learningContent);
            else
            {
                throw new ApplicationException("LearningElement is placed but has a different or null parent");
            }
        }
    }

    /// <summary>
    /// Calls the the show learning element content method for the selected learning element.
    /// </summary>
    /// <param name="learningElement"></param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected or no learning element
    /// is selected in the learning world.</exception>
    public async Task ShowSelectedElementContentAsync(ILearningElementViewModel learningElement)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        SetSelectedLearningElement(learningElement);
        await _presentationLogic.ShowLearningElementContentAsync((LearningElementViewModel)learningElement);
    }

    /// <summary>
    /// Deletes the given learning element.
    /// </summary>
    /// <param name="learningElement">Learning element to delete.</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void DeleteLearningElement(ILearningElementViewModel learningElement)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.DeleteLearningElementInWorld(LearningWorldVm, learningElement);
    }

    public IEnumerable<ILearningContentViewModel> GetAllContent() => _presentationLogic.GetAllContent();

    public void CreateUnplacedLearningElement(string name,
        ILearningContentViewModel learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.CreateUnplacedLearningElement(LearningWorldVm, name, learningContent,
            description, goals, difficulty, workload, points);
    }

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    public event PropertyChangingEventHandler? PropertyChanging;

    protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    private bool BeforeSetField<T>(T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        OnPropertyChanging(propertyName);
        return true;
    }
}