using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.Components;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Shared;
using ModalDialogOnCloseResult = Presentation.Components.ModalDialog.ModalDialogOnCloseResult;
using ModalDialogReturnValue = Presentation.Components.ModalDialog.ModalDialogReturnValue;

namespace Presentation.PresentationLogic.World;

public class WorldPresenter : IWorldPresenter, IWorldPresenterToolboxInterface, IWorldPresenterOverviewInterface
{
    public WorldPresenter(
        IPresentationLogic presentationLogic, ISpacePresenter spacePresenter,
        ILogger<WorldPresenter> logger, IAuthoringToolWorkspaceViewModel authoringToolWorkspaceViewModel)
    {
        _spacePresenter = spacePresenter;
        _presentationLogic = presentationLogic;
        _logger = logger;
        authoringToolWorkspaceViewModel.PropertyChanged += OnWorkspacePropertyChanged;
        //first-time update in case a world was selected before we were instantiated 
        WorldVm = authoringToolWorkspaceViewModel.SelectedWorld;
    }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ISpacePresenter _spacePresenter;
    private readonly ILogger<WorldPresenter> _logger;
    
    
    private IWorldViewModel? _worldVm;
    private IObjectInPathWayViewModel? _newConditionSourceObject;
    private SpaceViewModel? _newConditionTargetSpace;
    private bool _editSpaceDialogOpen;
    private bool _editPathWayConditionDialogOpen;
    private Dictionary<string, string>? _editSpaceDialogInitialValues;
    private Dictionary<string, string>? _editConditionDialogInitialValues;
    private Dictionary<string, string>? _editSpaceDialogAnnotations;
    private bool _createSpaceDialogOpen;
    private bool _createPathWayConditionDialogOpen;
    private int _spaceCreationCounter = 0;
    private int _conditionCreationCounter = 0;

    public bool SelectedObjectIsSpace =>
        WorldVm?.SelectedObject?.GetType() == typeof(SpaceViewModel);

    public bool ShowingSpaceView => WorldVm is { ShowingSpaceView: true };
    
    /// <summary>
    /// The currently selected WorldViewModel.
    /// </summary>
    public IWorldViewModel? WorldVm
    {
        get => _worldVm;
        internal set
        {
            var selectedObjectIsSpaceBefore = SelectedObjectIsSpace;
            var showingSpaceViewBefore = ShowingSpaceView;
            if (!BeforeSetField(_worldVm, value))
                return;
            SetField(ref _worldVm, value);
            if (_worldVm != null)
                _worldVm.PropertyChanged += _spacePresenter.OnWorldPropertyChanged;
            if (SelectedObjectIsSpace != selectedObjectIsSpaceBefore)
                OnPropertyChanged(nameof(SelectedObjectIsSpace));
            if (ShowingSpaceView != showingSpaceViewBefore)
                OnPropertyChanged(nameof(ShowingSpaceView));
            HideRightClickMenu();
        }
    }


    public bool CreatePathWayConditionDialogOpen
    {
        get => _createPathWayConditionDialogOpen;
        private set => SetField(ref _createPathWayConditionDialogOpen, value);
    }
    
    public bool EditSpaceDialogOpen
    {
        get => _editSpaceDialogOpen;
        private set => SetField(ref _editSpaceDialogOpen, value);
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

    public bool CreateSpaceDialogOpen
    {
        get => _createSpaceDialogOpen;
        private set => SetField(ref _createSpaceDialogOpen, value);
    }

    /// <inheritdoc cref="ISpaceNamesProvider.SpaceNames"/>
    public IEnumerable<string>? SpaceNames =>
        WorldVm?.Spaces.Select(space => space.Name);

    /// <inheritdoc cref="ISpaceNamesProvider.SpaceShortnames"/>
    public IEnumerable<string>? SpaceShortnames =>
        WorldVm?.Spaces.Select(space => space.Shortname);

    public event Action OnUndoRedoPerformed
    {
        add => _presentationLogic.OnUndoRedoPerformed += value;
        remove => _presentationLogic.OnUndoRedoPerformed -= value;
    }

    public void DragObjectInPathWay(object sender, DraggedEventArgs<IObjectInPathWayViewModel> args)
    {
        _presentationLogic.DragObjectInPathWay(args.DraggableObject, args.OldPositionX, args.OldPositionY);
        HideRightClickMenu();
    }

    /// <summary>
    /// If any object in the World has an active RightClickMenu, this object is set in this variable.
    /// Otherwise, it is null.
    /// </summary>
    public IObjectInPathWayViewModel? RightClickedObject { get; private set; }
    public void EditObjectInPathWay(IObjectInPathWayViewModel obj)
    {
        SetSelectedObject(obj);
        OpenEditSelectedObjectDialog();
    }

    public void DeleteSpace(ISpaceViewModel obj)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        _presentationLogic.DeleteSpace(WorldVm, obj);
    }

    public void DeleteDraggableObject(IObjectInPathWayViewModel obj)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        switch (obj)
        {
            case PathWayConditionViewModel pathWayConditionViewModel:
                _presentationLogic.DeletePathWayCondition(WorldVm, pathWayConditionViewModel);
                break;
            case SpaceViewModel spaceViewModel:
                _presentationLogic.DeleteSpace(WorldVm, spaceViewModel);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj));
        }
    }

    public void RightClickOnObjectInPathWay(IObjectInPathWayViewModel obj)
    {
        RightClickedObject = obj;
    }

    public void ClickOnObjectInWorld(ISelectableObjectInWorldViewModel obj)
    {
        SetSelectedObject(obj);
    }

    public void DoubleClickOnSpaceInWorld(IObjectInPathWayViewModel obj)
    {
        SetSelectedObject(obj);
        ShowSelectedSpaceView();
    }
    
    public void SwitchPathWayCondition(PathWayConditionViewModel pathWayCondition)
    {
        _presentationLogic.EditPathWayCondition(pathWayCondition, pathWayCondition.Condition == ConditionEnum.And ? ConditionEnum.Or : ConditionEnum.And);
    }

    public void HideRightClickMenu()
    {
        RightClickedObject = null;
    }

    public void AddNewSpace()
    {
        CreateSpaceDialogOpen = true;
    }

    public void OnWorkspacePropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AuthoringToolWorkspaceViewModel.SelectedWorld))
        {
            if (caller is not IAuthoringToolWorkspaceViewModel workspaceVm)
                throw new ArgumentException("Caller must be of type IAuthoringToolWorkspaceViewModel");
        
            WorldVm = workspaceVm.SelectedWorld;
        }
    }

    public void ShowSelectedSpaceView()
    {
        if (WorldVm != null) WorldVm.ShowingSpaceView = true;
        HideRightClickMenu();
    }

    public void CloseSpaceView()
    {
        if (WorldVm != null) WorldVm.ShowingSpaceView = false;
    }
    public void OpenEditSelectedObjectDialog()
    {
        if (_worldVm == null)
            throw new ApplicationException("SelectedWorld is null");

        switch (_worldVm.SelectedObject)
        {
            case null:
                return;
            case PathwayViewModel:
                return;
            case PathWayConditionViewModel:
                OpenEditSelectedPathWayConditionDialog();
                break;
            case SpaceViewModel:
                OpenEditSelectedSpaceDialog();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region Space

    /// <summary>
    /// Creates a new space in the currently selected world.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="shortname"></param>
    /// <param name="authors"></param>
    /// <param name="description"></param>
    /// <param name="goals"></param>
    /// <param name="requiredPoints"></param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <exception cref="ApplicationException">Thrown if no world is currently selected.</exception>
    public void CreateNewSpace(string name, string shortname,
        string authors, string description, string goals, int requiredPoints, double positionX = 0, double positionY = 0)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        _presentationLogic.CreateSpace(WorldVm, name, shortname, authors, description, goals, requiredPoints, positionX, positionY);
        //TODO: Return error in the command in case of failure
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected Space.
    /// </summary>
    private void OpenEditSelectedSpaceDialog()
    {
        var space = (SpaceViewModel) WorldVm?.SelectedObject!;
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
        EditSpaceDialogOpen = true;
    }
    
    /// <inheritdoc cref="IWorldPresenterToolboxInterface.AddSpace"/>
    public void AddSpace(ISpaceViewModel space)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        _presentationLogic.AddSpace(WorldVm, space);
    }

    /// <summary>
    /// Calls the LoadSpaceAsync methode in <see cref="_presentationLogic"/> and adds the returned space to the current world.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if <see cref="WorldVm"/> is null</exception>
    public async Task LoadSpaceAsync()
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        await _presentationLogic.LoadSpaceAsync(WorldVm);
    }

    /// <summary>
    /// Creates a space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok</exception>
    public void OnCreateSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        CreateSpaceDialogOpen = false;

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
        CreateNewSpace(name, shortname, authors, description, goals, requiredPoints, offset, offset);
    }

    /// <summary>
    /// Changes property values of space viewmodel with return values from the dialog.
    /// </summary>
    /// <param name="returnValueTuple">Return values from the dialog</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">Thrown if the dictionary in return values of dialog null while return value is ok
    /// or if the selected object not a space</exception>
    public void OnEditSpaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        EditSpaceDialogOpen = false;

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

        if (WorldVm == null)
            throw new ApplicationException("World is null");
        _spacePresenter.EditSpace(name, shortname, authors, description, goals, requiredPoints);
    }

    /// <summary>
    /// Deletes the selected object in the currently selected world and sets an other space or element as selected object.
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no world is currently selected.</exception>
    /// <exception cref="NotImplementedException">Thrown if the selected object is of an other type than space or element.</exception>
    public void DeleteSelectedObject()
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        switch (WorldVm.SelectedObject)
        {
            case null:
                return;
            case SpaceViewModel space:
                _presentationLogic.DeleteSpace(WorldVm, space);
                break;
            case PathwayViewModel pathWay:
                _presentationLogic.DeletePathWay(WorldVm, pathWay);
                break;
            case PathWayConditionViewModel condition:
                _presentationLogic.DeletePathWayCondition(WorldVm, condition);
                break;
        }
    }

    /// <summary>
    /// Calls the respective Save methode for Space or Element depending on which object is selected
    /// </summary>
    /// <exception cref="ApplicationException">Thrown if no world is currently selected.</exception>
    /// <exception cref="ApplicationException">Thrown if no space is currently selected.</exception>
    public async Task SaveSelectedSpaceAsync()
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        if (WorldVm.SelectedObject == null)
            throw new ApplicationException("SelectedSpace is null");
            
        await _presentationLogic.SaveSpaceAsync((SpaceViewModel)WorldVm.SelectedObject);
    }

    /// <summary>
    /// Changes the selected <see cref="IObjectInPathWayViewModel"/> in the currently selected world.
    /// </summary>
    /// <param name="pathWayObject">The pathway object that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no world is currently selected.</exception>
    internal void SetSelectedObject(ISelectableObjectInWorldViewModel pathWayObject)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        WorldVm.SelectedObject = pathWayObject;
        if (SelectedObjectIsSpace)
            _spacePresenter.SetSpace((SpaceViewModel)WorldVm.SelectedObject);
        else
        {
            WorldVm.SelectedObject = pathWayObject;
        }
        HideRightClickMenu();
    }

    #endregion

    #region PathWay

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
        if (WorldVm == null)
            throw new ApplicationException("World is null");
        var (response, data) = (returnValueTuple.ReturnValue, returnValueTuple.InputFieldValues);
        CreatePathWayConditionDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel)
        {
            _newConditionSourceObject = null;
            _newConditionTargetSpace = null;
            return;
        }
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            Console.Write($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        if (Enum.TryParse(data["Condition"], out ConditionEnum condition) == false)
            throw new ApplicationException("Condition is not a valid enum value");

        if (_newConditionSourceObject != null && _newConditionTargetSpace != null)
        {
            _presentationLogic.CreatePathWayConditionBetweenObjects(WorldVm, condition, _newConditionSourceObject, _newConditionTargetSpace);
            _newConditionSourceObject = null;
            _newConditionTargetSpace = null;
            return;
        }
        
        var offset = 15 * _conditionCreationCounter;
        _conditionCreationCounter = (_conditionCreationCounter + 1) % 10;
        _presentationLogic.CreatePathWayCondition(WorldVm, condition, offset+20, offset+100);
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected condition.
    /// </summary>
    private void OpenEditSelectedPathWayConditionDialog()
    {
        var condition = (PathWayConditionViewModel) WorldVm?.SelectedObject!;
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
    /// or if the selected object not a space.</exception>
    public void OnEditPathWayConditionDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        if (WorldVm == null)
            throw new ApplicationException("World is null");
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
        if (WorldVm.SelectedObject is not PathWayConditionViewModel pathWayCondition)
            throw new ApplicationException("DraggableObject is not a pathWayCondition");
        _presentationLogic.EditPathWayCondition(pathWayCondition, condition);
    }

    public void DeletePathWayCondition(PathWayConditionViewModel pathWayCondition)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        _presentationLogic.DeletePathWayCondition(WorldVm, pathWayCondition);
    }

    /// <summary>
    /// Sets the on hovered object of the world to the target object on the given position.
    /// When there is no object on the given position, the on hovered object is set to null.
    /// </summary>
    /// <param name="sourceObject">The object from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target object.</param>
    /// <param name="y">The y-coordinate of the target object.</param>
    public void SetOnHoveredObjectInPathWay(IObjectInPathWayViewModel sourceObject, double x, double y)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        var objectAtPosition = GetObjectAtPosition(x, y);
        if (objectAtPosition == null || objectAtPosition == sourceObject)
        {
            WorldVm.OnHoveredObjectInPathWay = null;
        }
        else
        
        {
            WorldVm.OnHoveredObjectInPathWay = objectAtPosition;
            _logger.LogDebug("ObjectAtPosition: {0} ", sourceObject.Id);
        }
    }
    
    /// <summary>
    /// Localizes and returns the object at the given position in the currently selected world.
    /// </summary>
    /// <param name="x">The x-coordinate of the target object</param>
    /// <param name="y">The y-coordinate of the target object</param>
    /// <returns>The pathway object at the given position.</returns>
    private IObjectInPathWayViewModel? GetObjectAtPosition(double x, double y)
    {
        //WorldVm can not be null because it is checked before call. -m.ho
        var objectAtPosition = WorldVm?.Spaces.FirstOrDefault(ls =>
                                   ls.PositionX <= x && ls.PositionX + 84 >= x && ls.PositionY <= y && ls.PositionY + 84 >= y) ??
                               (IObjectInPathWayViewModel?)WorldVm?.PathWayConditions.FirstOrDefault(lc =>
                                   lc.PositionX <= x && lc.PositionX + 76 >= x && lc.PositionY <= y && lc.PositionY + 43 >= y);
        return objectAtPosition;
    }
    
    /// <summary>
    /// Creates a pathway from the given source space to the target space on the given position.
    /// Does nothing when there is no space on the given position.
    /// </summary>
    /// <param name="sourceObject">The space from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target space</param>
    /// <param name="y">The y-coordinate of the target space</param>
    public void CreatePathWay(IObjectInPathWayViewModel sourceObject, double x, double y)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        var targetObject = GetObjectAtPosition( x, y);
        if (targetObject == null || targetObject == sourceObject)
            return;
        WorldVm.OnHoveredObjectInPathWay = null;
        if (targetObject.InBoundObjects.Count == 1 && targetObject is SpaceViewModel space)
        {
            _newConditionSourceObject = sourceObject;
            _newConditionTargetSpace = space;
            _createPathWayConditionDialogOpen = true;
            return;
        }
        _presentationLogic.CreatePathWay(WorldVm, sourceObject, targetObject);
    }

    /// <summary>
    /// Deletes the last created pathway leading to the target space.
    /// </summary>
    /// <param name="targetObject">The space where the path ends.</param>
    public void DeletePathWay(IObjectInPathWayViewModel targetObject)
    {
        if (WorldVm == null)
            throw new ApplicationException("SelectedWorld is null");
        var pathWay = WorldVm.PathWays.LastOrDefault(lp => lp.TargetObject == targetObject);
        if (pathWay == null)
            throw new ApplicationException("PathWay is null");
        _presentationLogic.DeletePathWay(WorldVm, pathWay);
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