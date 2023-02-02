using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// The AuthoringToolWorkspacePresenter is the central component that controls changes to the <see cref="AuthoringToolWorkspaceViewModel"/>.
/// </summary>
public class AuthoringToolWorkspacePresenter : IAuthoringToolWorkspacePresenter, IAuthoringToolWorkspacePresenterToolboxInterface
{
    public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IPresentationLogic presentationLogic, ISpacePresenter spacePresenter,
        ILogger<AuthoringToolWorkspacePresenter> logger, IShutdownManager shutdownManager)
    {
        _spacePresenter = spacePresenter;
        AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _shutdownManager = shutdownManager;
        CreateWorldDialogOpen = false;
        EditWorldDialogOpen = false;
        CreateSpaceDialogOpen = false;
        EditSpaceDialogOpen = false;
        DeletedUnsavedWorld = null;
        InformationMessageToShow = null;
        if (presentationLogic.RunningElectron) 
            //register callback so we can check for unsaved data on quit
            //TODO: register to our own quit button
            shutdownManager.BeforeShutdown += OnBeforeShutdown;
    }

    public IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get;}
    
    private readonly IPresentationLogic _presentationLogic;
    private readonly ISpacePresenter _spacePresenter;
    private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;
    private readonly IShutdownManager _shutdownManager;

    public bool CreateWorldDialogOpen { get; set; }
    public bool EditWorldDialogOpen { get; set; }

    internal bool CreateSpaceDialogOpen { get; set; }
    internal bool EditSpaceDialogOpen { get; set; }

    public bool SaveUnsavedChangesDialogOpen { get; set; }

    public bool WorldSelected => AuthoringToolWorkspaceVm.SelectedWorld != null;

    public Queue<WorldViewModel>? UnsavedWorldsQueue { get; set; }
    public WorldViewModel? DeletedUnsavedWorld { get; set; }
    public string? InformationMessageToShow { get; set; }

    /// <summary>
    /// This event is fired when a new <see cref="WorldViewModel"/> is created and the newly created
    /// world is passed.
    /// </summary>
    internal event EventHandler<WorldViewModel?>? OnWorldCreate;

    /// <summary>
    /// This event is fired when the selected world changed and the new
    /// selected <see cref="WorldViewModel"/> is passed.
    /// </summary>
    internal event EventHandler<WorldViewModel?>? OnWorldSelect;

    /// <summary>
    /// This event is fired when <see cref="DeleteSelectedWorld"/> is called successfully and the deleted
    /// world is passed.
    /// </summary>
    internal event EventHandler<WorldViewModel?>? OnWorldDelete;

    public event Action? OnForceViewUpdate;


    #region World

    public void AddNewWorld()
    {
        CreateWorldDialogOpen = true;
    }

    /// <summary>
    /// Sets the selected <see cref="WorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="worldName">The name of the world that should be selected.</param>
    /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
    public void SetSelectedWorld(string worldName)
    {
        var world = AuthoringToolWorkspaceVm.Worlds.FirstOrDefault(world => world.Name == worldName);
        if (world == null) throw new ArgumentException("no world with that name in viewmodel");
        AuthoringToolWorkspaceVm.SelectedWorld = world;
        OnWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedWorld);
    }

    /// <summary>
    /// Sets the selected <see cref="WorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="world">The world that should be set as selected</param>
    internal void SetSelectedWorld(WorldViewModel? world)
    {
        AuthoringToolWorkspaceVm.SelectedWorld = world;
        OnWorldSelect?.Invoke(this, AuthoringToolWorkspaceVm.SelectedWorld);
    }

    /// <summary>
    /// Deletes the currently selected world from the view model and selects the last world in the
    /// collection, if any remain.
    /// </summary>
    public void DeleteSelectedWorld()
    {
        var world = AuthoringToolWorkspaceVm.SelectedWorld;
        if (world == null) return;
        _presentationLogic.DeleteWorld(AuthoringToolWorkspaceVm, world);
        if (world.UnsavedChanges) DeletedUnsavedWorld = world;
        OnWorldDelete?.Invoke(this, world);
    }

    public void OpenEditSelectedWorldDialog()
    {
        if (AuthoringToolWorkspaceVm.SelectedWorld == null)
        {
            throw new ApplicationException("SelectedWorld is null");
        }

        //prepare dictionary property to pass to dialog
        AuthoringToolWorkspaceVm.EditDialogInitialValues = new Dictionary<string, string>
        {
            {"Name", AuthoringToolWorkspaceVm.SelectedWorld.Name},
            {"Shortname", AuthoringToolWorkspaceVm.SelectedWorld.Shortname},
            {"Authors", AuthoringToolWorkspaceVm.SelectedWorld.Authors},
            {"Language", AuthoringToolWorkspaceVm.SelectedWorld.Language},
            {"Description", AuthoringToolWorkspaceVm.SelectedWorld.Description},
            {"Goals", AuthoringToolWorkspaceVm.SelectedWorld.Goals},
        };
        EditWorldDialogOpen = true;
    }
    
    public void AddWorld(WorldViewModel world)
    {
        _presentationLogic.AddWorld(AuthoringToolWorkspaceVm, world);
    }

    public async Task LoadWorldAsync()
    {
        await _presentationLogic.LoadWorldAsync(AuthoringToolWorkspaceVm);
    }

    internal async Task SaveWorldAsync(WorldViewModel world)
    {
        await _presentationLogic.SaveWorldAsync(world);
    }

    public async Task SaveSelectedWorldAsync()
    {
        if (AuthoringToolWorkspaceVm.SelectedWorld == null)
            throw new ApplicationException("SelectedWorld is null");
        await SaveWorldAsync(AuthoringToolWorkspaceVm.SelectedWorld);
    }

    public void OnCreateWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        CreateWorldDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            _logger.LogTrace("{PairKey}:{PairValue}\\n", pair.Key, pair.Value);
        }

        //required arguments
        var name = data["Name"];
        //optional arguments
        var shortname = data.ContainsKey("Shortname") ? data["Shortname"] : "";
        var language = data.ContainsKey("Language") ? data["Language"] : "";
        var description = data.ContainsKey("Description") ? data["Description"] : "";
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        _presentationLogic.CreateWorld(AuthoringToolWorkspaceVm, name, shortname, authors, language, description, goals);
        OnWorldCreate?.Invoke(this, AuthoringToolWorkspaceVm.SelectedWorld);
    }

    public void OnEditWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var (response, data) = returnValueTuple;
        EditWorldDialogOpen = false;

        if (response == ModalDialogReturnValue.Cancel) return;
        if (data == null) throw new ApplicationException("dialog data unexpectedly null after Ok return value");

        foreach (var pair in data)
        {
            _logger.LogTrace("{PairKey}:{PairValue}\\n", pair.Key, pair.Value);
        }

        //required arguments
        var name = data["Name"];
        var shortname = data["Shortname"];
        var language = data["Language"];
        var description = data["Description"];
        //optional arguments
        var authors = data.ContainsKey("Authors") ? data["Authors"] : "";
        var goals = data.ContainsKey("Goals") ? data["Goals"] : "";
        
        if (AuthoringToolWorkspaceVm.SelectedWorld == null)
            throw new ApplicationException("SelectedWorld is null");
        _presentationLogic.EditWorld(AuthoringToolWorkspaceVm.SelectedWorld, name, shortname, authors, language, description, goals);
    }

    internal void OnBeforeShutdown(object? _, BeforeShutdownEventArgs args)
    {
        if (SaveUnsavedChangesDialogOpen)
        {
            args.CancelShutdown();
            return;
        }

        if (!AuthoringToolWorkspaceVm.Worlds.Any(lw => lw.UnsavedChanges)) return;
        args.CancelShutdown();
        UnsavedWorldsQueue =
            new Queue<WorldViewModel>(
                AuthoringToolWorkspaceVm.Worlds.Where(lw => lw.UnsavedChanges));
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

    public Task ProcessDragAndDropResult(Tuple<string, MemoryStream> result)
    {
        var (name, stream) = result;
        var ending = name.Split(".").Last().ToLower();
        switch (ending)
        {
            case "awf":
                LoadWorldFromFileStream(stream);
                break;
            case "asf":
                LoadSpaceFromFileStream(stream);
                break;
            case "aef":
                //TODO: Elements should be dropped into specific Slot. At the moment they are loaded into Slot 0. - AW
                LoadElementFromFileStream(stream, 0);
                break;
            case "jpg":
            case "png":
            case "webp":
            case "bmp":
            case "txt":
            case "c":
            case "h":
            case "cpp":
            case "cc":
            case "c++":
            case "py":
            case "cs":
            case "js":
            case "php":
            case "html":
            case "css":
            case "mp4":
            case "h5p":
            case "pdf":
                var content = _presentationLogic.LoadContentViewModel(name, stream);
                CallCreateElementWithPreloadedContentFromActiveView(content);
                break;
            default:
                _logger.LogInformation("Couldn\'t load file {Name} because the file extension {Ending} is not supported", name, ending);
                InformationMessageToShow =
                    $"Couldn't load file '{name}', because the file extension '{ending}' is not supported.";
                break;
        }

        return Task.CompletedTask;
    }

    internal void CallCreateElementWithPreloadedContentFromActiveView(ContentViewModel content)
    {
        if (AuthoringToolWorkspaceVm.SelectedWorld is not { } world) return;
        if (world.ShowingSpaceView)
        {
            if (_spacePresenter.SpaceVm is not { }) return;
            _spacePresenter.CreateElementWithPreloadedContent(content);
        }
        else
        {
            InformationMessageToShow = "Elements can only get loaded into spaces.";
        }
    }

    internal void LoadWorldFromFileStream(Stream stream)
    {
        _presentationLogic.LoadWorldViewModel(AuthoringToolWorkspaceVm, stream);
    }

    internal void LoadSpaceFromFileStream(Stream stream)
    {
        if (AuthoringToolWorkspaceVm.SelectedWorld == null)
        {
            InformationMessageToShow = "A world must be selected to import a space.";
            return;
        }
        _presentationLogic.LoadSpaceViewModel(AuthoringToolWorkspaceVm.SelectedWorld, stream);
    }

    internal void LoadElementFromFileStream(Stream stream, int slotIndex)
    {
        if (AuthoringToolWorkspaceVm.SelectedWorld is not { } world)
        {
            InformationMessageToShow = "A world must be selected to import a element.";
            return;
        }
        if (world.ShowingSpaceView)
        {
            if (_spacePresenter.SpaceVm is not { } space)
            {
                throw new ApplicationException(
                    $"ShowingSpaceView for World '{world.Name}' is true, but SpaceVm in SpacePresenter is null");
            }

            _presentationLogic.LoadElementViewModel(space, slotIndex, stream);
        }
        else
        {
            InformationMessageToShow = "Elements can only get loaded into spaces.";
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
                    SaveWorldAsync(world).Wait();
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

    public void OnSaveDeletedWorldDialogClose(ModalDialogOnCloseResult returnValueTuple)
    {
        var returnValue = returnValueTuple.ReturnValue;
        if (DeletedUnsavedWorld == null)
            throw new ApplicationException("SaveDeletedWorld modal returned value despite DeleteUnsavedWorld being null");
        switch (returnValue)
        {
            case ModalDialogReturnValue.Yes:
            {
                SaveWorldAsync(DeletedUnsavedWorld).Wait();
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