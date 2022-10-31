﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Shared;
using ModalDialogOnCloseResult = Presentation.Components.ModalDialog.ModalDialogOnCloseResult;
using ModalDialogReturnValue = Presentation.Components.ModalDialog.ModalDialogReturnValue;

namespace Presentation.PresentationLogic.LearningWorld;

public class LearningWorldPresenter : ILearningWorldPresenter, ILearningWorldPresenterToolboxInterface
{
    public LearningWorldPresenter(
        IPresentationLogic presentationLogic, ILearningSpacePresenter learningSpacePresenter,
        ILogger<LearningWorldPresenter> logger)
    {
        _learningSpacePresenter = learningSpacePresenter;
        _presentationLogic = presentationLogic;
        _logger = logger;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILogger<LearningWorldPresenter> _logger;
    
    
    private ILearningWorldViewModel? _learningWorldVm;
    private LearningContentViewModel? _dragAndDropLearningContent;
    private bool _editLearningSpaceDialogOpen;
    private bool _editPathWayConditionDialogOpen;
    private Dictionary<string, string>? _editSpaceDialogInitialValues;
    private Dictionary<string, string>? _editConditionDialogInitialValues;
    private Dictionary<string, string>? _editSpaceDialogAnnotations;
    private bool _createLearningSpaceDialogOpen;
    private bool _createPathWayConditionDialogOpen;
    private int _spaceCreationCounter = 0;
    private int _conditionCreationCounter = 0;

    public bool SelectedLearningObjectIsSpace =>
        LearningWorldVm?.SelectedLearningObject?.GetType() == typeof(LearningSpaceViewModel);

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
            if (_learningWorldVm != null)
                _learningWorldVm.PropertyChanged += _learningSpacePresenter.OnWorldPropertyChanged;
            if (SelectedLearningObjectIsSpace != selectedLearningObjectIsSpaceBefore)
                OnPropertyChanged(nameof(SelectedLearningObjectIsSpace));
            if (ShowingLearningSpaceView != showingLearningSpaceViewBefore)
                OnPropertyChanged(nameof(ShowingLearningSpaceView));
        }
    }

    public LearningContentViewModel? DragAndDropLearningContent
    {
        get => _dragAndDropLearningContent;
        private set => SetField(ref _dragAndDropLearningContent, value);
    }

    public bool CreatePathWayConditionDialogOpen
    {
        get => _createPathWayConditionDialogOpen;
        private set => SetField(ref _createPathWayConditionDialogOpen, value);
    }
    
    public bool EditLearningSpaceDialogOpen
    {
        get => _editLearningSpaceDialogOpen;
        private set => SetField(ref _editLearningSpaceDialogOpen, value);
    }

    public bool EditPathWayConditionDialogOpen 
    {
        get => _editPathWayConditionDialogOpen;
        private set => SetField(ref _editPathWayConditionDialogOpen, value);
    }

    public Dictionary<string, string>? EditSpaceDialogInitialValues
    {
        get => _editSpaceDialogInitialValues;
        private set => SetField(ref _editSpaceDialogInitialValues, value);
    }
    
    public Dictionary<string, string>? EditConditionDialogInitialValues
    {
        get => _editConditionDialogInitialValues;
        private set => SetField(ref _editConditionDialogInitialValues, value);
    }
    
    public Dictionary<string, string>? EditSpaceDialogAnnotations
    {
        get => _editSpaceDialogAnnotations;
        private set => SetField(ref _editSpaceDialogAnnotations, value);
    }

    public bool CreateLearningSpaceDialogOpen
    {
        get => _createLearningSpaceDialogOpen;
        private set => SetField(ref _createLearningSpaceDialogOpen, value);
    }

    public event Action OnUndoRedoPerformed
    {
        add => _presentationLogic.OnUndoRedoPerformed += value;
        remove => _presentationLogic.OnUndoRedoPerformed -= value;
    }

    public void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> args)
    {
        _presentationLogic.DragObjectInPathWay(args.LearningObject, args.OldPositionX, args.OldPositionY);
    }

    public void AddNewLearningSpace()
    {
        CreateLearningSpaceDialogOpen = true;
    }

    public void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AuthoringToolWorkspaceViewModel.SelectedLearningWorld))
        {
            if (caller is not IAuthoringToolWorkspaceViewModel workspaceVm)
                throw new ArgumentException("Caller must be of type IAuthoringToolWorkspaceViewModel");
        
            LearningWorldVm = workspaceVm.SelectedLearningWorld;
        }
    }

    public void ShowSelectedLearningSpaceView()
    {
        if (LearningWorldVm != null) LearningWorldVm.ShowingLearningSpaceView = true;
    }

    public void CloseLearningSpaceView()
    {
        if (LearningWorldVm != null) LearningWorldVm.ShowingLearningSpaceView = false;
    }
    public void OpenEditSelectedObjectDialog()
    {
        if (_learningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");

        switch (_learningWorldVm.SelectedLearningObject)
        {
            case null:
                return;
            case LearningPathwayViewModel:
                return;
            case PathWayConditionViewModel:
                OpenEditSelectedPathWayConditionDialog();
                break;
            case LearningSpaceViewModel:
                OpenEditSelectedLearningSpaceDialog();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region LearningSpace

    /// <summary>
    /// Creates a new learning space in the currently selected learning world.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void CreateNewLearningSpace(string name, string shortname,
        string authors, string description, string goals, int requiredPoints, double positionX = 0, double positionY = 0)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.CreateLearningSpace(LearningWorldVm, name, shortname, authors, description, goals, requiredPoints, positionX, positionY);
        //TODO: Return error in the command in case of failure
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected LearningSpace.
    /// </summary>
    private void OpenEditSelectedLearningSpaceDialog()
    {
        var space = (LearningSpaceViewModel) LearningWorldVm?.SelectedLearningObject!;
        //prepare dictionary property to pass to dialog
        EditSpaceDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", space.Name},
            {"Shortname", space.Shortname},
            {"Authors", space.Authors},
            {"Description", space.Description},
            {"Goals", space.Goals},
            {"Required Points", space.RequiredPoints.ToString()},
        };
        EditSpaceDialogAnnotations = new Dictionary<string, string>
        {
            {"Required Points", "/" + space.Points.ToString()},
        };
        EditLearningSpaceDialogOpen = true;
    }
    
    /// <inheritdoc cref="ILearningWorldPresenterToolboxInterface.AddLearningSpace"/>
    public void AddLearningSpace(ILearningSpaceViewModel learningSpace)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.AddLearningSpace(LearningWorldVm, learningSpace);
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
    /// Creates a learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok</exception>
    public void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        CreateLearningSpaceDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            Console.Write($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        var name = data["Name"];
        //optional arguments
        var shortname = data.ContainsKey("Shortname") ? data["Shortname"] : "";
        var description = data.ContainsKey("Description") ? data["Description"] : "";
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        var requiredPoints = data.ContainsKey("Required Points") && data["Required Points"] != "" && !data["Required Points"].StartsWith("e") ? int.Parse(data["Required Points"]) : 0;
        var offset = 15 * _spaceCreationCounter;
        _spaceCreationCounter = (_spaceCreationCounter + 1) % 10;
        CreateNewLearningSpace(name, shortname, authors, description, goals, requiredPoints, offset, offset);
    }

    /// <summary>
    /// Changes property values of learning space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if the selected learning object not a learning space</exception>
    public void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        EditLearningSpaceDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var (key, value) in data)
        {
            _logger.LogTrace("{Key}:{Value}\\n", key, value);
        }

        //required arguments
        var name = data["Name"];
        //optional arguments
        var shortname = data.ContainsKey("Shortname") ? data["Shortname"] : "";
        var description = data.ContainsKey("Description") ? data["Description"] : "";
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        var requiredPoints = data.ContainsKey("Required Points") && data["Required Points"] != "" && !data["Required Points"].StartsWith("e") ? int.Parse(data["Required Points"]) : 0;

        if (LearningWorldVm == null)
            throw new ApplicationException("LearningWorld is null");
        _learningSpacePresenter.EditLearningSpace(name, shortname, authors, description, goals, requiredPoints);
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
        switch (LearningWorldVm.SelectedLearningObject)
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

    /// <summary>
    /// Calls the respective Save methode for Learning Space or Learning Element depending on which learning object is selected
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    /// <exception cref="ApplicationException">Thrown if no learning space is currently selected.</exception>
    public async Task SaveSelectedLearningSpaceAsync()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (LearningWorldVm.SelectedLearningObject == null)
                throw new ApplicationException("SelectedLearningSpace is null");
            
        await _presentationLogic.SaveLearningSpaceAsync((LearningSpaceViewModel)LearningWorldVm.SelectedLearningObject);
    }

    /// <summary>
    /// Changes the selected <see cref="IObjectInPathWayViewModel"/> in the currently selected learning world.
    /// </summary>
    /// <param name="pathWayObject">The pathway object that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void SetSelectedLearningObject(ISelectableObjectInWorldViewModel pathWayObject)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        LearningWorldVm.SelectedLearningObject = pathWayObject;
        if (SelectedLearningObjectIsSpace)
            _learningSpacePresenter.SetLearningSpace((LearningSpaceViewModel)LearningWorldVm.SelectedLearningObject);
        else
        {
            LearningWorldVm.SelectedLearningObject = pathWayObject;
        }
    }

    #endregion

    #region LearningPathWay

    public void AddNewPathWayCondition()
    {
        CreatePathWayConditionDialogOpen = true;
    }
    
    /// <summary>
    /// Creates a pathway condition viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok</exception>
    public void OnCreatePathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("LearningWorld is null");
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        CreatePathWayConditionDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            Console.Write($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        if (Enum.TryParse(data["Condition"], out ConditionEnum condition) == false)
            throw new ApplicationException("Condition is not a valid enum value");
        var offset = 15 * _conditionCreationCounter;
        _conditionCreationCounter = (_conditionCreationCounter + 1) % 10;
        _presentationLogic.CreatePathWayCondition(LearningWorldVm, condition, offset+20, offset+100);
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected condition.
    /// </summary>
    private void OpenEditSelectedPathWayConditionDialog()
    {
        var condition = (PathWayConditionViewModel) LearningWorldVm?.SelectedLearningObject!;
        //prepare dictionary property to pass to dialog
        EditConditionDialogInitialValues = new Dictionary<string, string>
        {
            {"Condition", condition.Condition.ToString()},
        };
        EditPathWayConditionDialogOpen = true;
    }
    
    /// <summary>
    /// Changes property condition of pathway condition viewmodel with return value from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog.</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if the selected learning object not a learning space.</exception>
    public void OnEditPathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("LearningWorld is null");
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        EditPathWayConditionDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var (key, value) in data)
        {
            _logger.LogTrace("{Key}:{Value}\\n", key, value);
        }

        //required arguments
        if (Enum.TryParse(data["Condition"], out ConditionEnum condition) == false)
            throw new ApplicationException("Condition is not a valid enum value");
        if (LearningWorldVm.SelectedLearningObject is not PathWayConditionViewModel pathWayCondition)
            throw new ApplicationException("LearningObject is not a pathWayCondition");
        _presentationLogic.EditPathWayCondition(pathWayCondition, condition);
    }

    /// <summary>
    /// Sets the on hovered learning object of the learning world to the target object on the given position.
    /// When there is no learning object on the given position, the on hovered learning object is set to null.
    /// </summary>
    /// <param name="sourceObject">The learning object from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target object.</param>
    /// <param name="y">The y-coordinate of the target object.</param>
    public void SetOnHoveredLearningObject(IObjectInPathWayViewModel sourceObject, double x, double y)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var objectAtPosition = GetObjectAtPosition(x, y);
        if (objectAtPosition == null || objectAtPosition == sourceObject)
        {
            LearningWorldVm.OnHoveredLearningObject = null;
        }
        else
        
        {
            LearningWorldVm.OnHoveredLearningObject = objectAtPosition;
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
                                   ls.PositionX <= x && ls.PositionX + 100 >= x && ls.PositionY <= y && ls.PositionY + 50 >= y) ??
                               (IObjectInPathWayViewModel?)LearningWorldVm?.PathWayConditions.FirstOrDefault(lc =>
                                   Math.Pow(x - lc.PositionX, 2) + Math.Pow(y - lc.PositionY, 2) <= 400);
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
        var targetObject = GetObjectAtPosition( x, y);
        if (targetObject == null || targetObject == sourceObject)
            return;
        LearningWorldVm.OnHoveredLearningObject = null;
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