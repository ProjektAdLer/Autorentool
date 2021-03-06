using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// The AuthoringToolWorkspacePresenter is the central component that controls changes to the <see cref="AuthoringToolWorkspaceViewModel"/>.
/// </summary>
public class AuthoringToolWorkspacePresenter : IAuthoringToolWorkspacePresenterToolboxInterface
{
    public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IPresentationLogic presentationLogic,
        ILearningWorldPresenter learningWorldPresenter, ILearningSpacePresenter learningSpacePresenter,
        ILearningElementPresenter learningElementPresenter, ILogger<AuthoringToolWorkspacePresenter> logger,
        IShutdownManager shutdownManager)
    {
        _learningSpacePresenter = learningSpacePresenter;
        _learningElementPresenter = learningElementPresenter;
        _learningWorldPresenter = learningWorldPresenter;
        _authoringToolWorkspaceVm = authoringToolWorkspaceVm;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _shutdownManager = shutdownManager;
        CreateLearningWorldDialogOpen = false;
        EditLearningWorldDialogOpen = false;
        CreateLearningSpaceDialogueOpen = false;
        EditLearningSpaceDialogOpen = false;
        WorldToReplaceWith = null;
        ReplacedUnsavedWorld = null;
        DeletedUnsavedWorld = null;
        InformationMessageToShow = null;
        OnLearningWorldSelect += _learningWorldPresenter.SetLearningWorld;
        if (!presentationLogic.RunningElectron) return;
        //register callback so we can check for unsaved data on quit
        //TODO: register to our own quit button
        shutdownManager.BeforeShutdown += OnBeforeShutdown;
    }

    private readonly IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceVm;
    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningWorldPresenter _learningWorldPresenter;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILearningElementPresenter _learningElementPresenter;
    private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;
    private readonly IShutdownManager _shutdownManager;

    internal bool CreateLearningWorldDialogOpen { get; set; }
    internal bool EditLearningWorldDialogOpen { get; set; }

    internal bool CreateLearningSpaceDialogueOpen { get; set; }
    internal bool EditLearningSpaceDialogOpen { get; set; }

    internal bool SaveUnsavedChangesDialogOpen { get; set; }

    internal bool LearningWorldSelected => _authoringToolWorkspaceVm.SelectedLearningWorld != null;

    internal Queue<LearningWorldViewModel>? UnsavedWorldsQueue;
    internal LearningWorldViewModel? WorldToReplaceWith { get; set; }
    internal LearningWorldViewModel? ReplacedUnsavedWorld { get; set; }
    internal LearningWorldViewModel? DeletedUnsavedWorld { get; set; }
    internal string? InformationMessageToShow { get; set; }

    /// <summary>
    /// This event is fired when <see cref="CreateNewLearningWorld"/> is called successfully and the newly created
    /// world is passed.
    /// </summary>
    internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldCreate;

    /// <summary>
    /// This event is fired when <see cref="ChangeSelectedLearningWorld"/> is called successfully and the new
    /// selection is passed.
    /// </summary>
    internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldSelect;

    /// <summary>
    /// This event is fired when <see cref="DeleteSelectedLearningWorld"/> is called successfully and the deleted
    /// world is passed.
    /// </summary>
    internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldDelete;

    /// <summary>
    /// This event is fired when <see cref="EditSelectedLearningWorld(string,string,string,string,string,string)"/> is called by the modal dialog as a callback.
    /// The newly edited learning world is passed.
    /// </summary>
    internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldEdit;

    internal event Action? OnForceViewUpdate;


    #region LearningWorld

    /// <summary>
    /// Creates a new LearningWorld in our ViewModel.
    /// </summary>
    /// <param name="name">The name of the world.</param>
    /// <param name="shortname">The short name of the world.</param>
    /// <param name="authors">A list of authors of the world.</param>
    /// <param name="language">The primary language of the world.</param>
    /// <param name="description">A description of the world.</param>
    /// <param name="goals">The goals of the world.</param>
    internal void CreateNewLearningWorld(string name, string shortname, string authors, string language,
        string description, string goals)
    {
        //TODO: check if world with that name exists already? is name our unique identifier?
        var learningWorld =
            _learningWorldPresenter.CreateNewLearningWorld(name, shortname, authors, language, description, goals);
        _authoringToolWorkspaceVm.AddLearningWorld(learningWorld);
        OnLearningWorldCreate?.Invoke(this, learningWorld);
    }

    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="worldName">The name of the world that should be selected.</param>
    /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
    internal void SetSelectedLearningWorld(string worldName)
    {
        var world = _authoringToolWorkspaceVm.LearningWorlds.FirstOrDefault(world => world.Name == worldName);
        if (world == null) throw new ArgumentException("no world with that name in viewmodel");
        _authoringToolWorkspaceVm.SelectedLearningWorld = world;
        OnLearningWorldSelect?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
    }

    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="learningWorld">The learning world that should be set as selected</param>
    internal void SetSelectedLearningWorld(LearningWorldViewModel? learningWorld)
    {
        _authoringToolWorkspaceVm.SelectedLearningWorld = learningWorld;
        OnLearningWorldSelect?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
    }

    /// <summary>
    /// Deletes the currently selected learning world from the view model and selects the last learning world in the
    /// collection, if any remain.
    /// </summary>
    public void DeleteSelectedLearningWorld()
    {
        var learningWorld = _authoringToolWorkspaceVm.SelectedLearningWorld;
        if (learningWorld == null) return;
        _authoringToolWorkspaceVm.RemoveLearningWorld(learningWorld);
        if (learningWorld.UnsavedChanges) DeletedUnsavedWorld = learningWorld;
        SetSelectedLearningWorld(_authoringToolWorkspaceVm.LearningWorlds.LastOrDefault());
        OnLearningWorldDelete?.Invoke(this, learningWorld);
    }

    public void OpenEditSelectedLearningWorldDialog()
    {
        if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
        {
            throw new ApplicationException("SelectedLearningWorld is null");
        }

        //prepare dictionary property to pass to dialog
        _authoringToolWorkspaceVm.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", _authoringToolWorkspaceVm.SelectedLearningWorld.Name},
            {"Shortname", _authoringToolWorkspaceVm.SelectedLearningWorld.Shortname},
            {"Authors", _authoringToolWorkspaceVm.SelectedLearningWorld.Authors},
            {"Language", _authoringToolWorkspaceVm.SelectedLearningWorld.Language},
            {"Description", _authoringToolWorkspaceVm.SelectedLearningWorld.Description},
            {"Goals", _authoringToolWorkspaceVm.SelectedLearningWorld.Goals},
        };
        EditLearningWorldDialogOpen = true;
    }

    /// <summary>
    /// Edits the currently selected learning world to have the passed values.
    /// </summary>
    /// <param name="name">The name of the world.</param>
    /// <param name="shortname">The short name of the world.</param>
    /// <param name="authors">A list of authors of the world.</param>
    /// <param name="language">The primary language of the world.</param>
    /// <param name="description">A description of the world.</param>
    /// <param name="goals">The goals of the world.</param>
    /// <exception cref="ApplicationException">Thrown if now learning world is currently selected.</exception>
    public void EditSelectedLearningWorld(string name, string shortname, string authors, string language,
        string description, string goals)
    {
        if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _authoringToolWorkspaceVm.SelectedLearningWorld =
            _learningWorldPresenter.EditLearningWorld(_authoringToolWorkspaceVm.SelectedLearningWorld, name,
                shortname, authors, language, description, goals);
        OnLearningWorldEdit?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
    }
    
    public void AddLearningWorld(LearningWorldViewModel learningWorld)
    {
        if (_authoringToolWorkspaceVm.LearningWorlds.Any(world => world.Name == learningWorld.Name))
        {
            WorldToReplaceWith = learningWorld;
            return;
        }

        _authoringToolWorkspaceVm.AddLearningWorld(learningWorld);
    }

    public async Task LoadLearningWorldAsync()
    {
        var learningWorld = await _presentationLogic.LoadLearningWorldAsync();
        AddLearningWorld(learningWorld);
    }

    public void ReplaceLearningWorld(LearningWorldViewModel toReplace)
    {
        var toBeReplaced = _authoringToolWorkspaceVm.LearningWorlds.First(world => world.Name == toReplace.Name);
        _authoringToolWorkspaceVm.RemoveLearningWorld(toBeReplaced);
        if (toBeReplaced.UnsavedChanges)
        {
            if (ReplacedUnsavedWorld != null)
                throw new ApplicationException("multiple unsaved replaced worlds, this should not happen");
            ReplacedUnsavedWorld = toBeReplaced;
        }

        _authoringToolWorkspaceVm.AddLearningWorld(toReplace);
        if (_authoringToolWorkspaceVm.SelectedLearningWorld == toBeReplaced)
        {
            _authoringToolWorkspaceVm.SelectedLearningWorld = toReplace;
            OnLearningWorldSelect?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
        }
    }

    public async Task SaveLearningWorldAsync(LearningWorldViewModel world)
    {
        await _presentationLogic.SaveLearningWorldAsync(world);
    }

    public async Task SaveSelectedLearningWorldAsync()
    {
        if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        await SaveLearningWorldAsync(_authoringToolWorkspaceVm.SelectedLearningWorld);
    }

    public Task OnCreateWorldDialogClose(
        Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        CreateLearningWorldDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            _logger.LogTrace($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        var language = data["Language"];
        var description = data["Description"];
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        CreateNewLearningWorld(name, shortname, authors, language, description, goals);
        return Task.CompletedTask;
    }

    public Task OnEditWorldDialogClose(Tuple<ModalDialogReturnValue, IDictionary<string, string>?> returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        EditLearningWorldDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return Task.CompletedTask;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        //TODO: change this into a trace ILogger call
        foreach (var pair in data)
        {
            _logger.LogTrace($"{pair.Key}:{pair.Value}\n");
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        var language = data["Language"];
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";

        EditSelectedLearningWorld(name, shortname, authors, language, description, goals);
        return Task.CompletedTask;
    }

    public IEnumerable<ModalDialogInputField> ModalDialogWorldInputFields
    {
        get
        {
            return new ModalDialogInputField[]
            {
                new("Name", ModalDialogInputType.Text, true),
                new("Shortname", ModalDialogInputType.Text, true),
                new("Authors", ModalDialogInputType.Text),
                new("Language", ModalDialogInputType.Text, true),
                new("Description", ModalDialogInputType.Text, true),
                new("Goals", ModalDialogInputType.Text)
            };
        }
    }


    internal void OnBeforeShutdown(object? _, BeforeShutdownEventArgs args)
    {
        if (SaveUnsavedChangesDialogOpen)
        {
            args.CancelShutdown();
            return;
        }

        if (!_authoringToolWorkspaceVm.LearningWorlds.Any(lw => lw.UnsavedChanges)) return;
        args.CancelShutdown();
        UnsavedWorldsQueue =
            new Queue<LearningWorldViewModel>(
                _authoringToolWorkspaceVm.LearningWorlds.Where(lw => lw.UnsavedChanges));
        SaveUnsavedChangesDialogOpen = true;

        OnForceViewUpdate?.Invoke();
    }

    internal void CompletedSaveQueue(bool cancelled = false)
    {
        UnsavedWorldsQueue = null;
        SaveUnsavedChangesDialogOpen = false;
        if (!cancelled)
            _shutdownManager.BeginShutdown();
    }

    #endregion

    #region DragAndDrop

    public Task ProcessDragAndDropResult(Tuple<string, Stream> result)
    {
        var (name, stream) = result;
        var ending = name.Split(".").Last().ToLower();
        switch (ending)
        {
            case "awf":
                LoadLearningWorldFromFileStream(stream);
                break;
            case "asf":
                LoadLearningSpaceFromFileStream(stream);
                break;
            case "aef":
                LoadLearningElementFromFileStream(stream);
                break;
            case "jpg":
            case "png":
            case "webp":
            case "bmp":
            case "mp4":
            case "h5p":
            case "pdf":
                var learningContent = _presentationLogic.LoadLearningContentViewModelFromStream(name, stream);
                CallCreateLearningElementWithPreloadedContentFromActiveView(learningContent);
                break;
            default:
                _logger.LogInformation($"Couldn't load file '{name}', because the file extension '{ending}' is not supported.");
                InformationMessageToShow =
                    $"Couldn't load file '{name}', because the file extension '{ending}' is not supported.";
                break;
        }

        return Task.CompletedTask;
    }

    internal void CallCreateLearningElementWithPreloadedContentFromActiveView(LearningContentViewModel learningContent)
    {
        if (_authoringToolWorkspaceVm.SelectedLearningWorld is not { } world) return;
        if (world.ShowingLearningSpaceView)
        {
            if (_learningSpacePresenter.LearningSpaceVm is not { } space) return;
            _learningSpacePresenter.CreateLearningElementWithPreloadedContent(learningContent);
        }
        else
        {
            _learningWorldPresenter.CreateLearningElementWithPreloadedContent(learningContent);
        }
    }

    internal void LoadLearningWorldFromFileStream(Stream stream)
    {
        var learningWorld =
            _presentationLogic.LoadLearningWorldViewModelFromStream(stream);
        if (_authoringToolWorkspaceVm.LearningWorlds.Any(w => w.Name == learningWorld.Name))
        {
            WorldToReplaceWith = learningWorld;
        }

        _authoringToolWorkspaceVm.AddLearningWorld(learningWorld);
        _authoringToolWorkspaceVm.SelectedLearningWorld ??= learningWorld;
        OnLearningWorldSelect?.Invoke(this, _authoringToolWorkspaceVm.SelectedLearningWorld);
    }

    internal void LoadLearningSpaceFromFileStream(Stream stream)
    {
        var learningSpace =
            _presentationLogic.LoadLearningSpaceViewModelFromStream(stream);
        if (_authoringToolWorkspaceVm.SelectedLearningWorld == null)
        {
            InformationMessageToShow = "A learning world must be selected to import a learning space.";
            return;
        }
        _authoringToolWorkspaceVm.SelectedLearningWorld.LearningSpaces.Add(learningSpace);
        _authoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject = learningSpace;
    }

    internal void LoadLearningElementFromFileStream(Stream stream)
    {
        var learningElement =
            _presentationLogic.LoadLearningElementViewModelFromStream(stream);
        if (_authoringToolWorkspaceVm.SelectedLearningWorld is not { } world)
        {
            InformationMessageToShow = "A learning world must be selected to import a learning element.";
            return;
        }
        if (world.ShowingLearningSpaceView)
        {
            if (_learningSpacePresenter.LearningSpaceVm is not { } space) return;
            learningElement.Parent = space;
            space.LearningElements.Add(learningElement);
            space.SelectedLearningObject = learningElement;
        }
        else
        {
            learningElement.Parent = world;
            world.LearningElements.Add(learningElement);
            world.SelectedLearningObject = learningElement;
        }
    }
    
    #endregion
}