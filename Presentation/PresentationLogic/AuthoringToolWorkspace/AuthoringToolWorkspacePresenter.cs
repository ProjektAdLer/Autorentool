using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// The AuthoringToolWorkspacePresenter is the central component that controls changes to the <see cref="AuthoringToolWorkspaceViewModel"/>.
/// </summary>
public class AuthoringToolWorkspacePresenter : IAuthoringToolWorkspacePresenter, IAuthoringToolWorkspacePresenterToolboxInterface
{
    public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IPresentationLogic presentationLogic,
        ILearningWorldPresenter learningWorldPresenter, ILearningSpacePresenter learningSpacePresenter,
        ILogger<AuthoringToolWorkspacePresenter> logger, IShutdownManager shutdownManager)
    {
        _learningSpacePresenter = learningSpacePresenter;
        _learningWorldPresenter = learningWorldPresenter;
        AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _shutdownManager = shutdownManager;
        CreateLearningWorldDialogOpen = false;
        EditLearningWorldDialogOpen = false;
        CreateLearningSpaceDialogOpen = false;
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

    public IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get;}
    
    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningWorldPresenter _learningWorldPresenter;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;
    private readonly IShutdownManager _shutdownManager;

    public bool CreateLearningWorldDialogOpen { get; set; }
    public bool EditLearningWorldDialogOpen { get; set; }

    internal bool CreateLearningSpaceDialogOpen { get; set; }
    internal bool EditLearningSpaceDialogOpen { get; set; }

    public bool SaveUnsavedChangesDialogOpen { get; set; }

    public bool LearningWorldSelected => AuthoringToolWorkspaceVm.SelectedLearningWorld != null;

    public Queue<LearningWorldViewModel>? UnsavedWorldsQueue { get; set; }
    public LearningWorldViewModel? WorldToReplaceWith { get; set; }
    public LearningWorldViewModel? ReplacedUnsavedWorld { get; set; }
    public LearningWorldViewModel? DeletedUnsavedWorld { get; set; }
    public string? InformationMessageToShow { get; set; }

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

    public event Action? OnForceViewUpdate;


    #region LearningWorld

    public void AddNewLearningWorld()
    {
        CreateLearningWorldDialogOpen = true;
    }

    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="worldName">The name of the world that should be selected.</param>
    /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
    public void SetSelectedLearningWorld(string worldName)
    {
        var world = AuthoringToolWorkspaceVm.LearningWorlds.FirstOrDefault(world => world.Name == worldName);
        if (world == null) throw new ArgumentException("no world with that name in viewmodel");
        AuthoringToolWorkspaceVm.SelectedLearningWorld = world;
        OnLearningWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
    }

    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="learningWorld">The learning world that should be set as selected</param>
    internal void SetSelectedLearningWorld(LearningWorldViewModel? learningWorld)
    {
        AuthoringToolWorkspaceVm.SelectedLearningWorld = learningWorld;
        OnLearningWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
    }

    /// <summary>
    /// Deletes the currently selected learning world from the view model and selects the last learning world in the
    /// collection, if any remain.
    /// </summary>
    public void DeleteSelectedLearningWorld()
    {
        var learningWorld = AuthoringToolWorkspaceVm.SelectedLearningWorld;
        if (learningWorld == null) return;
        _presentationLogic.DeleteLearningWorld(AuthoringToolWorkspaceVm, learningWorld);
        if (learningWorld.UnsavedChanges) DeletedUnsavedWorld = learningWorld;
        SetSelectedLearningWorld(AuthoringToolWorkspaceVm.LearningWorlds.LastOrDefault());
        OnLearningWorldDelete?.Invoke(this, learningWorld);
    }

    public void OpenEditSelectedLearningWorldDialog()
    {
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld == null)
        {
            throw new ApplicationException("SelectedLearningWorld is null");
        }

        //prepare dictionary property to pass to dialog
        AuthoringToolWorkspaceVm.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", AuthoringToolWorkspaceVm.SelectedLearningWorld.Name},
            {"Shortname", AuthoringToolWorkspaceVm.SelectedLearningWorld.Shortname},
            {"Authors", AuthoringToolWorkspaceVm.SelectedLearningWorld.Authors},
            {"Language", AuthoringToolWorkspaceVm.SelectedLearningWorld.Language},
            {"Description", AuthoringToolWorkspaceVm.SelectedLearningWorld.Description},
            {"Goals", AuthoringToolWorkspaceVm.SelectedLearningWorld.Goals},
        };
        EditLearningWorldDialogOpen = true;
    }
    
    public void AddLearningWorld(LearningWorldViewModel learningWorld)
    {
        if (AuthoringToolWorkspaceVm.LearningWorlds.Any(world => world.Name == learningWorld.Name))
        {
            WorldToReplaceWith = learningWorld;
            return;
        }

        AuthoringToolWorkspaceVm.AddLearningWorld(learningWorld);
    }

    public async Task LoadLearningWorldAsync()
    {
        var learningWorld = await _presentationLogic.LoadLearningWorldAsync();
        AddLearningWorld(learningWorld);
    }

    internal void ReplaceLearningWorld(LearningWorldViewModel toReplace)
    {
        var toBeReplaced = AuthoringToolWorkspaceVm.LearningWorlds.First(world => world.Name == toReplace.Name);
        AuthoringToolWorkspaceVm.RemoveLearningWorld(toBeReplaced);
        if (toBeReplaced.UnsavedChanges)
        {
            if (ReplacedUnsavedWorld != null)
                throw new ApplicationException("multiple unsaved replaced worlds, this should not happen");
            ReplacedUnsavedWorld = toBeReplaced;
        }

        AuthoringToolWorkspaceVm.AddLearningWorld(toReplace);
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld == toBeReplaced)
        {
            AuthoringToolWorkspaceVm.SelectedLearningWorld = toReplace;
            OnLearningWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
        }
    }

    internal async Task SaveLearningWorldAsync(LearningWorldViewModel world)
    {
        await _presentationLogic.SaveLearningWorldAsync(world);
    }

    public async Task SaveSelectedLearningWorldAsync()
    {
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        await SaveLearningWorldAsync(AuthoringToolWorkspaceVm.SelectedLearningWorld);
    }

    public void OnCreateWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        CreateLearningWorldDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
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
        _presentationLogic.CreateLearningWorld(AuthoringToolWorkspaceVm, name, shortname, authors, language, description, goals);
        OnLearningWorldCreate?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
        OnLearningWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
    }

    public void OnEditWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        EditLearningWorldDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
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
        
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        _presentationLogic.EditLearningWorld(AuthoringToolWorkspaceVm.SelectedLearningWorld, name, shortname, authors, language, description, goals);
        OnLearningWorldEdit?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
    }

    internal void OnBeforeShutdown(object? _, BeforeShutdownEventArgs args)
    {
        if (SaveUnsavedChangesDialogOpen)
        {
            args.CancelShutdown();
            return;
        }

        if (!AuthoringToolWorkspaceVm.LearningWorlds.Any(lw => lw.UnsavedChanges)) return;
        args.CancelShutdown();
        UnsavedWorldsQueue =
            new Queue<LearningWorldViewModel>(
                AuthoringToolWorkspaceVm.LearningWorlds.Where(lw => lw.UnsavedChanges));
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
                var learningContent = _presentationLogic.LoadLearningContentViewModel(name, stream);
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
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld is not { } world) return;
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
            _presentationLogic.LoadLearningWorldViewModel(stream);
        if (AuthoringToolWorkspaceVm.LearningWorlds.Any(w => w.Name == learningWorld.Name))
        {
            WorldToReplaceWith = learningWorld;
        }

        AuthoringToolWorkspaceVm.AddLearningWorld(learningWorld);
        AuthoringToolWorkspaceVm.SelectedLearningWorld ??= learningWorld;
        OnLearningWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
    }

    internal void LoadLearningSpaceFromFileStream(Stream stream)
    {
        var learningSpace =
            _presentationLogic.LoadLearningSpaceViewModel(stream);
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld == null)
        {
            InformationMessageToShow = "A learning world must be selected to import a learning space.";
            return;
        }
        AuthoringToolWorkspaceVm.SelectedLearningWorld.LearningSpaces.Add(learningSpace);
        AuthoringToolWorkspaceVm.SelectedLearningWorld.SelectedLearningObject = learningSpace;
    }

    internal void LoadLearningElementFromFileStream(Stream stream)
    {
        var learningElement =
            _presentationLogic.LoadLearningElementViewModel(stream);
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld is not { } world)
        {
            InformationMessageToShow = "A learning world must be selected to import a learning element.";
            return;
        }
        if (world.ShowingLearningSpaceView)
        {
            if (_learningSpacePresenter.LearningSpaceVm is not { } space) return;
            learningElement.Parent = space;
            space.LearningElements.Add(learningElement);
            learningElement.Parent = space;
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

    public void OnSaveWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        if (UnsavedWorldsQueue == null)
            throw new ApplicationException("SaveUnsavedChanges modal returned value despite UnsavedWorldsQueue being null");
        var returnValue = returnValueTuple.ReturnValue;
        switch (returnValue)
        {
            case ModalDialogReturnValue.Cancel: //we want to cancel closing the application entirely
                CompletedSaveQueue(true);
                return;
            case ModalDialogReturnValue.Yes: //we want to save the world and iterate the queue
            {
                var world = UnsavedWorldsQueue.Dequeue();
                try
                {
                    SaveLearningWorldAsync(world).Wait();
                }
                catch (OperationCanceledException)
                {
                    CompletedSaveQueue(true);
                }
                break;
            }
            case ModalDialogReturnValue.No: //we do not want to save the world but iterate the queue anyway
            {
                var world = UnsavedWorldsQueue.Dequeue();
                world.UnsavedChanges = false;
                break;
            }
            case ModalDialogReturnValue.Ok:
            case ModalDialogReturnValue.Delete:
            default:
                throw new ArgumentOutOfRangeException(nameof(returnValueTuple), $"Unexpected return value of {returnValue}");
        }
        if (!UnsavedWorldsQueue.Any())
        {
            CompletedSaveQueue();
        }
    }

    public void OnSaveReplacedWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        if (ReplacedUnsavedWorld == null)
            throw new ApplicationException("SaveReplacedWorld modal returned value despite ReplacedUnsavedWorld being null");
        var returnValue = returnValueTuple.ReturnValue;
        switch (returnValue)
        {
            case ModalDialogReturnValue.Yes:
            {
                SaveLearningWorldAsync(ReplacedUnsavedWorld).Wait();
                break;
            }
            case ModalDialogReturnValue.No:
            {
                break;
            }
            case ModalDialogReturnValue.Ok:
            case ModalDialogReturnValue.Cancel:
            case ModalDialogReturnValue.Delete:
            default:
                throw new ArgumentOutOfRangeException(nameof(returnValueTuple), $"Unexpected return value of {returnValue}");
        }
        ReplacedUnsavedWorld = null;
    }

    public void OnReplaceDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var returnValue = returnValueTuple.ReturnValue;
        if (WorldToReplaceWith == null)
            throw new ApplicationException("WorldToReplaceWith was null but OnReplaceDialogClose was called");
        switch (returnValue)
        {
            case ModalDialogReturnValue.Ok:
            {
                ReplaceLearningWorld(WorldToReplaceWith);
                WorldToReplaceWith = null;
                break;
            }
            case ModalDialogReturnValue.Cancel:
            {
                WorldToReplaceWith = null;
                break;
            }
            case ModalDialogReturnValue.Delete:
            case ModalDialogReturnValue.Yes:
            case ModalDialogReturnValue.No:
            default:
                throw new ArgumentOutOfRangeException(nameof(returnValueTuple), $"Unexpected return value of {returnValue}");
        }
    }

    public void OnSaveDeletedWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var returnValue = returnValueTuple.ReturnValue;
        if (DeletedUnsavedWorld == null)
            throw new ApplicationException("SaveDeletedWorld modal returned value despite DeleteUnsavedWorld being null");
        switch (returnValue)
        {
            case ModalDialogReturnValue.Yes:
            {
                SaveLearningWorldAsync(DeletedUnsavedWorld).Wait();
                break;
            }
            case ModalDialogReturnValue.No:
            {
                break;
            }
            case ModalDialogReturnValue.Ok:
            case ModalDialogReturnValue.Cancel:
            case ModalDialogReturnValue.Delete:
            default:
                throw new ArgumentOutOfRangeException();
        }
        DeletedUnsavedWorld = null;
    }
}