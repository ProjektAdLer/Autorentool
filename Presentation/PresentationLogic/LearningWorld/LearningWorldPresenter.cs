using System.ComponentModel;
using System.Runtime.CompilerServices;
using AutoMapper;
using BusinessLogic.Validation;
using Presentation.Components;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningContent.Story;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.PresentationLogic.Topic;
using Shared;
using Shared.Command;

namespace Presentation.PresentationLogic.LearningWorld;

public class LearningWorldPresenter : ILearningWorldPresenter,
    ILearningWorldPresenterOverviewInterface
{
    private readonly IErrorService _errorService;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILogger<LearningWorldPresenter> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    private readonly IPresentationLogic _presentationLogic;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;

    private ILearningWorldViewModel? _learningWorldVm;

    public LearningWorldPresenter(
        IPresentationLogic presentationLogic, ILearningSpacePresenter learningSpacePresenter,
        ILogger<LearningWorldPresenter> logger, IMediator mediator,
        ISelectedViewModelsProvider selectedViewModelsProvider, IErrorService errorService,
        IMapper mapper)
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
        _mapper = mapper;
    }

    /// <inheritdoc cref="ILearningWorldPresenter.LearningWorldVm"/>
    public ILearningWorldViewModel? LearningWorldVm
    {
        get => _learningWorldVm;
        internal set
        {
            if (!BeforeSetField(_learningWorldVm, value))
                return;
            SetField(ref _learningWorldVm, value);
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

    public event PropertyChangedEventHandler? PropertyChanged;

    public event PropertyChangingEventHandler? PropertyChanging;

    private void OnSelectedViewModelsProviderOnPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(_selectedViewModelsProvider.LearningWorld)) return;
        if (caller is not ISelectedViewModelsProvider)
        {
            LogAndSetError("OnSelectedViewModelsProviderOnPropertyChanged",
                $"Caller must be of type ISelectedViewModelsProvider, got {caller?.GetType()}",
                "Caller must be of type ISelectedViewModelsProvider");
            return;
        }

        LearningWorldVm = _selectedViewModelsProvider.LearningWorld;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

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


    private bool CheckLearningWorldNotNull(string operation)
    {
        if (LearningWorldVm != null)
        {
            return true;
        }

        LogAndSetError(operation, "SelectedLearningWorld is null", "No learning world selected");
        return false;
    }

    private void LogAndSetError(string operation, string errorDetail, string userMessage)
    {
        _logger.LogError("Error in {Operation}: {ErrorDetail}", operation, errorDetail);
        _errorService.SetError("Operation failed", userMessage);
    }

    #region LearningWorld

    /// <inheritdoc cref="ILearningWorldPresenter.EditLearningWorld"/>
    public void EditLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, string evaluationLink, string enrolmentKey)
    {
        if (!CheckLearningWorldNotNull("EditLearningWorld"))
            return;

        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        _presentationLogic.EditLearningWorld(LearningWorldVm!, name, shortname, authors, language, description, goals,
            evaluationLink, enrolmentKey);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.SaveLearningWorld"/>
    public void SaveLearningWorld()
    {
        if (!CheckLearningWorldNotNull("SaveLearningWorldAsync"))
            return;

        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        _presentationLogic.SaveLearningWorld((LearningWorldViewModel)LearningWorldVm!);
    }

    #endregion

    #region ObjectInPathWay

    /// <summary>
    /// Changes the selected <see cref="IObjectInPathWayViewModel"/> in the currently selected learning world.
    /// </summary>
    /// <param name="pathWayObject">The pathway object that should be set as selected</param>
    internal void SetSelectedLearningObject(ISelectableObjectInWorldViewModel pathWayObject)
    {
        if (!CheckLearningWorldNotNull("SetSelectedLearningObject"))
            return;
        if (_selectedViewModelsProvider.LearningObjectInPathWay is LearningSpaceViewModel space)
        {
            _learningSpacePresenter.SetLearningSpace(
                space);
        }

        _selectedViewModelsProvider.SetLearningObjectInPathWay(pathWayObject, null);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DragObjectInPathWay"/>
    public void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> args)
    {
        _presentationLogic.DragObjectInPathWay(args.LearningObject, args.OldPositionX, args.OldPositionY);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.ClickOnObjectInWorld"/>
    public void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj)
    {
        SetSelectedLearningObject(obj);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DoubleClickOnObjectInPathway"/>
    public void DoubleClickOnObjectInPathway(IObjectInPathWayViewModel obj)
    {
        SetSelectedLearningObject(obj);
        if (obj is PathWayConditionViewModel pathwayCondition)
            SwitchPathWayCondition(pathwayCondition);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DeleteLearningObject"/>
    public void DeleteLearningObject(IObjectInPathWayViewModel objectInPathWayViewModel)
    {
        if (!CheckLearningWorldNotNull("DeleteLearningObject"))
            return;
        switch (objectInPathWayViewModel)
        {
            case PathWayConditionViewModel pathWayConditionViewModel:
                _presentationLogic.DeletePathWayCondition(LearningWorldVm!, pathWayConditionViewModel);
                break;
            case LearningSpaceViewModel learningSpaceViewModel:
                _presentationLogic.DeleteLearningSpace(LearningWorldVm!, learningSpaceViewModel);
                break;
        }
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DeleteSelectedLearningObject"/>
    public void DeleteSelectedLearningObject()
    {
        if (!CheckLearningWorldNotNull("SaveLearningWorldAsync"))
            return;
        switch (_selectedViewModelsProvider.LearningObjectInPathWay)
        {
            case null:
                return;
            case LearningSpaceViewModel space:
                _presentationLogic.DeleteLearningSpace(LearningWorldVm!, space);
                break;
            case LearningPathwayViewModel pathWay:
                _presentationLogic.DeleteLearningPathWay(LearningWorldVm!, pathWay);
                break;
            case PathWayConditionViewModel condition:
                _presentationLogic.DeletePathWayCondition(LearningWorldVm!, condition);
                break;
        }
    }

    #region LearningSpace

    /// <inheritdoc cref="ISelectedSetter.SetSelectedLearningSpace"/>
    public void SetSelectedLearningSpace(IObjectInPathWayViewModel objectInPathWayViewModel)
    {
        SetSelectedLearningObject(objectInPathWayViewModel);
        _selectedViewModelsProvider.SetLearningElement(null, null);
        _mediator.RequestOpenSpaceDialog();
    }

    /// <inheritdoc cref="ILearningWorldPresenter.CreateLearningSpace"/>
    public void CreateLearningSpace(string name, string description,
        LearningOutcomeCollectionViewModel learningOutcomeCollectionVm,
        int requiredPoints,
        Theme theme, TopicViewModel? topic = null)
    {
        if (!CheckLearningWorldNotNull("CreateLearningSpace"))
            return;

        var positionY = GetNextAvailableYPosition(25);

        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        _presentationLogic.CreateLearningSpace(LearningWorldVm!, name, description, learningOutcomeCollectionVm,
            requiredPoints, theme, 0, positionY, topic);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.AddNewLearningSpace"/>
    public void AddNewLearningSpace()
    {
        if (!CheckLearningWorldNotNull("AddNewLearningSpace"))
            return;
        _selectedViewModelsProvider.SetLearningObjectInPathWay(null, null);
        _mediator.RequestOpenSpaceDialog();
    }

    /// <inheritdoc cref="ILearningWorldPresenter.EditSelectedLearningSpace"/>
    public void EditSelectedLearningSpace()
    {
        if (!CheckLearningWorldNotNull("EditSelectedLearningSpace"))
            return;
        if (_selectedViewModelsProvider.LearningObjectInPathWay is not LearningSpaceViewModel)
        {
            LogAndSetError("EditSelectedLearningSpace", "SelectedLearningObjectInPathWay is not LearningSpaceViewModel",
                "Selected object in pathway is not a learning space");
            return;
        }

        _mediator.RequestOpenSpaceDialog();
    }

    public void CreateUnplacedLearningElementFromFormModel(LearningElementFormModel model)
    {
        CreateUnplacedLearningElement(model.Name, _mapper.Map<ILearningContentViewModel>(model.LearningContent),
            model.Description, model.Goals, model.Difficulty, model.ElementModel, model.Workload, model.Points);
    }

    public void EditLearningElementFromFormModel(ILearningSpaceViewModel? parent,
        ILearningElementViewModel elementToEdit,
        LearningElementFormModel model)
    {
        //map content except when it is an adaptivity content, as this is not editable in the normal form
        //in that case just take existing content in the element
        var content = elementToEdit.LearningContent is IAdaptivityContentViewModel && model.LearningContent == null
            ? elementToEdit.LearningContent
            : _mapper.Map<ILearningContentViewModel>(model.LearningContent);
        //unpack model and call edit
        EditLearningElement(parent, elementToEdit, model.Name, model.Description, model.Goals, model.Difficulty,
            model.ElementModel, model.Workload, model.Points, content);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DeleteLearningSpace"/>
    public void DeleteLearningSpace(ILearningSpaceViewModel learningSpaceViewModel)
    {
        if (!CheckLearningWorldNotNull("DeleteLearningSpace"))
            return;
        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        _presentationLogic.DeleteLearningSpace(LearningWorldVm!, learningSpaceViewModel);
    }

    #endregion

    #region PathWayCondition

    /// <inheritdoc cref="ILearningWorldPresenter.CreatePathWayCondition"/>
    public void CreatePathWayCondition(ConditionEnum condition = ConditionEnum.Or)
    {
        if (!CheckLearningWorldNotNull("CreatePathWayCondition"))
            return;
        try
        {
            var positionY = GetNextAvailableYPosition(25);
            _presentationLogic.CreatePathWayCondition(LearningWorldVm!, condition, 0, positionY);
        }
        catch (ApplicationException e)
        {
            _errorService.SetError("Error while creating PathWayCondition", e.Message);
        }
    }

    /// <inheritdoc cref="ILearningWorldPresenter.SwitchPathWayCondition"/>
    public void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition)
    {
        _presentationLogic.EditPathWayCondition(pathWayCondition,
            pathWayCondition.Condition == ConditionEnum.And ? ConditionEnum.Or : ConditionEnum.And);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DeletePathWayCondition"/>
    public void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition)
    {
        if (!CheckLearningWorldNotNull("DeletePathWayCondition"))
            return;
        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        _presentationLogic.DeletePathWayCondition(LearningWorldVm!, pathWayCondition);
    }

    #endregion

    #region LearningPathWay

    /// <inheritdoc cref="IPositioningService.SetOnHoveredObjectInPathWay"/>
    public void SetOnHoveredObjectInPathWay(IObjectInPathWayViewModel sourceObject, double x, double y)
    {
        if (!CheckLearningWorldNotNull("SetOnHoveredObjectInPathWay"))
            return;
        var objectAtPosition = GetObjectAtPosition(x, y);
        if (objectAtPosition == null || objectAtPosition == sourceObject)
        {
            LearningWorldVm!.OnHoveredObjectInPathWay = null;
        }
        else
        {
            LearningWorldVm!.OnHoveredObjectInPathWay = objectAtPosition;
            _logger.LogDebug("ObjectAtPosition: {Id} ", sourceObject.Id);
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
                                   ls.PositionX <= x && ls.PositionX + 80 >= x && ls.PositionY <= y &&
                                   ls.PositionY + 82 >= y) ??
                               (IObjectInPathWayViewModel?)LearningWorldVm?.PathWayConditions.FirstOrDefault(lc =>
                                   lc.PositionX <= x && lc.PositionX + 76 >= x && lc.PositionY <= y &&
                                   lc.PositionY + 41 >= y);
        return objectAtPosition;
    }

    /// <summary>
    /// Determines the next available Y position, taking into account objects already existing at the current position and the position shifted by an offset.
    /// </summary>
    /// <param name="xOffset">The horizontal offset to consider when identifying colliding objects.</param>
    /// <param name="startY">The Y starting point from which to begin the search. Defaults to 0.</param>
    /// <returns>Returns the next available Y position. If the determined Y position exceeds the maximum value, the maximum Y value is returned.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown object is found at the current position.</exception>
    private double GetNextAvailableYPosition(double xOffset, double startY = 0)
    {
        var positionY = startY;
        const double maxPositionY = 950;

        while (true)
        {
            var objAtPosition = GetObjectAtPosition(0, positionY);
            var objAtPositionWithOffset = GetObjectAtPosition(0 + xOffset, positionY + xOffset);

            if (objAtPosition == null && objAtPositionWithOffset == null)
            {
                return positionY <= maxPositionY ? positionY : maxPositionY;
            }

            var currentOffset = objAtPosition switch
            {
                ILearningSpaceViewModel => 85,
                PathWayConditionViewModel => 55,
                null when objAtPositionWithOffset is PathWayConditionViewModel condition => 55 +
                    (condition.PositionY - positionY),
                null when objAtPositionWithOffset is ILearningSpaceViewModel space =>
                    85 + (space.PositionY - positionY),
                _ => 85 + xOffset
            };

            positionY += currentOffset;
        }
    }

    /// <inheritdoc cref="IPositioningService.CreateLearningPathWay"/>
    public void CreateLearningPathWay(IObjectInPathWayViewModel sourceObject, double x, double y)
    {
        if (!CheckLearningWorldNotNull("CreateLearningPathWay"))
            return;
        var targetObject = GetObjectAtPosition(x, y);
        if (targetObject == null || targetObject == sourceObject)
            return;
        LearningWorldVm!.OnHoveredObjectInPathWay = null;
        if (targetObject.InBoundObjects.Count == 1 && targetObject is LearningSpaceViewModel)
        {
            return;
        }

        _presentationLogic.CreateLearningPathWay(LearningWorldVm, sourceObject, targetObject);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DeleteLearningPathWay"/>
    public void DeleteLearningPathWay(IObjectInPathWayViewModel targetObject)
    {
        if (!CheckLearningWorldNotNull("DeleteLearningPathWay"))
            return;

        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        var learningPathWay =
            _selectedViewModelsProvider.LearningObjectInPathWay is LearningPathwayViewModel learningPathwayViewModel
            && learningPathwayViewModel.TargetObject == targetObject
                ? learningPathwayViewModel
                : LearningWorldVm!.LearningPathWays.LastOrDefault(lp => lp.TargetObject == targetObject);
        ;


        if (learningPathWay == null)
        {
            LogAndSetError("DeleteLearningPathWay", "LearningPathWay is null", "No LearningPathWay found");
            return;
        }

        _presentationLogic.DeleteLearningPathWay(LearningWorldVm!, learningPathWay);
    }

    #endregion

    #endregion

    #region LearningElement

    /// <inheritdoc cref="ISelectedSetter.SetSelectedLearningElement"/>
    public void SetSelectedLearningElement(ILearningElementViewModel learningElement)
    {
        if (!CheckLearningWorldNotNull("SetSelectedLearningElement"))
            return;
        if (learningElement.Parent != null)
        {
            _selectedViewModelsProvider.SetLearningObjectInPathWay(learningElement.Parent, null);
        }

        _mediator.CloseAllLeftSide();

        switch (learningElement.LearningContent)
        {
            case AdaptivityContentViewModel:
                _mediator.RequestOpenAdaptivityElementDialog();
                break;
            case StoryContentViewModel:
                _mediator.RequestOpenStoryElementDialog();
                break;
            case FileContentViewModel or LinkContentViewModel:
                _mediator.RequestOpenElementDialog();
                break;
        }

        _selectedViewModelsProvider.SetLearningElement(learningElement, null);
    }

    /// <inheritdoc cref="ILearningWorldPresenter.EditLearningElement"/>
    public void EditLearningElement(ILearningSpaceViewModel? elementParent, ILearningElementViewModel learningElement,
        string name, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel,
        int workload, int points, ILearningContentViewModel learningContent)
    {
        if (!CheckLearningWorldNotNull("EditLearningElement"))
            return;
        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        if (LearningWorldVm!.UnplacedLearningElements.Contains(learningElement))
            if (learningElement.Parent == null)
                _presentationLogic.EditLearningElement(elementParent, learningElement, name,
                    description, goals, difficulty, elementModel, workload, points, learningContent);
            else
            {
                LogAndSetError("EditLearningElement", "LearningElement is unplaced but has a space parent",
                    "LearningElement is unplaced but has a space parent");
            }
        else
        {
            if (learningElement.Parent == _selectedViewModelsProvider.LearningObjectInPathWay)
                _learningSpacePresenter.EditLearningElement(learningElement, name, description, goals, difficulty,
                    elementModel, workload, points, learningContent);
            else
            {
                LogAndSetError("EditLearningElement", "LearningElement is placed but has a different or null parent",
                    "LearningElement is placed but has a different or null parent");
            }
        }
    }

    /// <inheritdoc cref="ILearningWorldPresenter.ShowSelectedElementContentAsync"/>
    public async Task ShowSelectedElementContentAsync(ILearningElementViewModel learningElement)
    {
        if (!CheckLearningWorldNotNull("ShowSelectedElementContentAsync"))
            return;
        SetSelectedLearningElement(learningElement);
        try
        {
            await _presentationLogic.ShowLearningElementContentAsync((LearningElementViewModel)learningElement);
        }
        catch (ArgumentOutOfRangeException e)
        {
            _errorService.SetError("Error while showing learning element content", e.Message);
        }
        catch (IOException e)
        {
            _errorService.SetError("Error while showing learning element content", e.Message);
        }
        catch (InvalidOperationException e)
        {
            _errorService.SetError("Error while showing learning element content", e.Message);
        }
    }

    /// <inheritdoc cref="ILearningWorldPresenter.DeleteLearningElement"/>
    public void DeleteLearningElement(ILearningElementViewModel learningElement)
    {
        if (!CheckLearningWorldNotNull("DeleteLearningElement"))
            return;
        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        _presentationLogic.DeleteLearningElementInWorld(LearningWorldVm!, learningElement);
    }

    public IEnumerable<ILearningContentViewModel> GetAllContent() => _presentationLogic.GetAllContent();

    /// <inheritdoc cref="ILearningWorldPresenter.CreateUnplacedLearningElement"/>
    public void CreateUnplacedLearningElement(string name,
        ILearningContentViewModel learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points)
    {
        if (!CheckLearningWorldNotNull("CreateUnplacedLearningElement"))
            return;
        //Nullability of LearningWorldVm is checked in CheckLearningWorldNotNull
        _presentationLogic.CreateUnplacedLearningElement(LearningWorldVm!, name, learningContent,
            description, goals, difficulty, elementModel, workload, points);
    }

    #endregion
}