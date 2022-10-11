using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
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
    private Dictionary<string, string>? _editSpaceDialogInitialValues;
    private Dictionary<string, string>? _editSpaceDialogAnnotations;
    private bool _createLearningSpaceDialogOpen;

    public bool SelectedLearningObjectIsSpace =>
        LearningWorldVm?.SelectedLearningSpace?.GetType() == typeof(LearningSpaceViewModel);

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

    public bool EditLearningSpaceDialogOpen
    {
        get => _editLearningSpaceDialogOpen;
        private set => SetField(ref _editLearningSpaceDialogOpen, value);
    }

    public Dictionary<string, string>? EditSpaceDialogInitialValues
    {
        get => _editSpaceDialogInitialValues;
        private set => SetField(ref _editSpaceDialogInitialValues, value);
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
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void CreateNewLearningSpace(string name, string shortname,
        string authors, string description, string goals, int requiredPoints)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.CreateLearningSpace(LearningWorldVm, name, shortname, authors, description, goals, requiredPoints);
        //TODO: Return error in the command in case of failure
    }

    /// <summary>
    /// Sets the initial values for the <see cref="ModalDialog"/> with the current values from the selected LearningSpace.
    /// </summary>
    public void OpenEditSelectedLearningSpaceDialog()
    {
        if(_learningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (_learningWorldVm.SelectedLearningSpace == null)
            return;
        var space = (LearningSpaceViewModel) LearningWorldVm?.SelectedLearningSpace!;
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
        var shortname = data["Shortname"];
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        var requiredPoints = data.ContainsKey("Required Points") && data["Required Points"] != "" && !data["Required Points"].StartsWith("e") ? int.Parse(data["Required Points"]) : 0;
        CreateNewLearningSpace(name, shortname, authors, description, goals, requiredPoints);
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
        var shortname = data["Shortname"];
        var description = data["Description"];
        //optional arguments
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
    public void DeleteSelectedLearningSpace()
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        if (LearningWorldVm.SelectedLearningSpace == null)
            return;
        _presentationLogic.DeleteLearningSpace(LearningWorldVm, LearningWorldVm.SelectedLearningSpace);
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
        if (LearningWorldVm.SelectedLearningSpace == null)
                throw new ApplicationException("SelectedLearningSpace is null");
            
        await _presentationLogic.SaveLearningSpaceAsync((LearningSpaceViewModel)LearningWorldVm.SelectedLearningSpace);
    }

    /// <summary>
    /// Changes the selected <see cref="ILearningSpaceViewModel"/> in the currently selected learning world.
    /// </summary>
    /// <param name="learningSpace">The learning space that should be set as selected</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    public void SetSelectedLearningSpace(ILearningSpaceViewModel learningSpace)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        LearningWorldVm.SelectedLearningSpace = learningSpace;
        if (SelectedLearningObjectIsSpace)
            _learningSpacePresenter.SetLearningSpace(LearningWorldVm.SelectedLearningSpace);
    }

    #endregion

    #region LearningPathWay

    /// <summary>
    /// Sets the on hovered learning space of the learning world to the target space on the given position.
    /// When there is no learning space on the given position, the on hovered learning space is set to null.
    /// </summary>
    /// <param name="sourceSpace">The learning space from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target space</param>
    /// <param name="y">The y-coordinate of the target space</param>
    public void SetOnHoveredLearningSpace(ILearningSpaceViewModel sourceSpace, double x, double y)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var objectAtPosition = GetObjectAtPosition(x, y);
        if (objectAtPosition == null || objectAtPosition == sourceSpace)
        {
            LearningWorldVm.OnHoveredLearningSpace = null;
        }
        else
        
        {
            LearningWorldVm.OnHoveredLearningSpace = objectAtPosition;
        }
    }
    
    /// <summary>
    /// Localizes and returns the learning space at the given position in the currently selected learning world.
    /// </summary>
    /// <param name="x">The x-coordinate of the target space</param>
    /// <param name="y">The y-coordinate of the target space</param>
    /// <returns>The learning space at the given position.</returns>
    private ILearningSpaceViewModel? GetObjectAtPosition(double x, double y)
    {
        //LearningWorldVm can not be null because it is checked before call. -m.ho
        var objectAtPosition = LearningWorldVm?.LearningSpaces
            .FirstOrDefault(ls => ls.PositionX <= x && ls.PositionX + 100 >= x 
                                                    && ls.PositionY <= y && ls.PositionY + 50 >= y);
        return objectAtPosition;
    }
    
    /// <summary>
    /// Creates a learning pathway from the given source space to the target space on the given position.
    /// Does nothing when there is no learning space on the given position.
    /// </summary>
    /// <param name="sourceSpace">The learning space from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target space</param>
    /// <param name="y">The y-coordinate of the target space</param>
    public void CreateLearningPathWay(ILearningSpaceViewModel sourceSpace, double x, double y)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        var targetSpace = GetObjectAtPosition(x, y);
        if (targetSpace == null || targetSpace == sourceSpace)
            return;
        LearningWorldVm.OnHoveredLearningSpace = null;
        _presentationLogic.CreateLearningPathWay(LearningWorldVm, sourceSpace, targetSpace);
    }

    /// <summary>
    /// Deletes the last created learning pathway leading to the target space.
    /// </summary>
    /// <param name="targetSpace">The learning space where the path ends.</param>
    public void DeleteLearningPathWay(ILearningSpaceViewModel targetSpace)
    {
        if (LearningWorldVm == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.DeleteLearningPathWay(LearningWorldVm, targetSpace);
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