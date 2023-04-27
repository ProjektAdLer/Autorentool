using MudBlazor;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// The AuthoringToolWorkspacePresenter is the central component that controls changes to the <see cref="AuthoringToolWorkspaceViewModel"/>.
/// </summary>
public class AuthoringToolWorkspacePresenter : IAuthoringToolWorkspacePresenter,
    IAuthoringToolWorkspacePresenterToolboxInterface
{
    public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IPresentationLogic presentationLogic, ILearningSpacePresenter learningSpacePresenter,
        ILogger<AuthoringToolWorkspacePresenter> logger, IMediator mediator, IShutdownManager shutdownManager,
        IDialogService dialogService)
    {
        _learningSpacePresenter = learningSpacePresenter;
        AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _shutdownManager = shutdownManager;
        _dialogService = dialogService;
        DeletedUnsavedWorld = null;
        _mediator = mediator;
        if (presentationLogic.RunningElectron)
            //register callback so we can check for unsaved data on quit
            //TODO: register to our own quit button
            shutdownManager.BeforeShutdown += OnBeforeShutdownAsync;
    }

    public IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;
    private readonly IMediator _mediator;
    private readonly IShutdownManager _shutdownManager;
    private readonly IDialogService _dialogService;

    public bool LearningWorldSelected => _mediator.SelectedLearningWorld != null;

    public LearningWorldViewModel? DeletedUnsavedWorld { get; set; }

    /// <summary>
    /// This event is fired when a new <see cref="LearningWorldViewModel"/> is created and the newly created
    /// world is passed.
    /// </summary>
    internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldCreate;

    /// <summary>
    /// This event is fired when the selected learning world changed and the new
    /// selected <see cref="LearningWorldViewModel"/> is passed.
    /// </summary>
    internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldSelect;

    /// <summary>
    /// This event is fired when <see cref="DeleteSelectedLearningWorld"/> is called successfully and the deleted
    /// world is passed.
    /// </summary>
    internal event EventHandler<LearningWorldViewModel?>? OnLearningWorldDelete;

    public event Action? OnForceViewUpdate;


    #region LearningWorld

    public void CreateLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals)
    {
        _presentationLogic.CreateLearningWorld(AuthoringToolWorkspaceVm, name, shortname, authors, language, description, goals);
        _mediator.SelectedLearningWorld = AuthoringToolWorkspaceVm.LearningWorlds.Last();
        OnLearningWorldCreate?.Invoke(this, _mediator.SelectedLearningWorld);
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
        _mediator.SelectedLearningWorld = world;
        OnLearningWorldSelect?.Invoke(this, _mediator.SelectedLearningWorld);
    }

    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="learningWorld">The learning world that should be set as selected</param>
    internal void SetSelectedLearningWorld(LearningWorldViewModel? learningWorld)
    {
        _mediator.SelectedLearningWorld = learningWorld;
        OnLearningWorldSelect?.Invoke(this, _mediator.SelectedLearningWorld);
    }

    /// <summary>
    /// Deletes the currently selected learning world from the view model and selects the last learning world in the
    /// collection, if any remain.
    /// </summary>
    public void DeleteSelectedLearningWorld()
    {
        var learningWorld = _mediator.SelectedLearningWorld;
        if (learningWorld == null) return;
        _presentationLogic.DeleteLearningWorld(AuthoringToolWorkspaceVm, learningWorld);
        if (learningWorld.UnsavedChanges) DeletedUnsavedWorld = learningWorld;
        OnLearningWorldDelete?.Invoke(this, learningWorld);
    }

    public void AddLearningWorld(LearningWorldViewModel learningWorld)
    {
        _presentationLogic.AddLearningWorld(AuthoringToolWorkspaceVm, learningWorld);
    }

    public async Task LoadLearningWorldAsync()
    {
        await _presentationLogic.LoadLearningWorldAsync(AuthoringToolWorkspaceVm);
    }

    internal async Task SaveLearningWorldAsync(LearningWorldViewModel world)
    {
        await _presentationLogic.SaveLearningWorldAsync(world);
    }

    public async Task SaveSelectedLearningWorldAsync()
    {
        if (_mediator.SelectedLearningWorld == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        await SaveLearningWorldAsync(_mediator.SelectedLearningWorld);
    }


    internal async Task OnBeforeShutdownAsync(object? sender, BeforeShutdownEventArgs args)
    {
        var unsavedWorlds = AuthoringToolWorkspaceVm.LearningWorlds.Where(WorldHasUnsavedChanges).ToList();
        _logger.LogInformation("Found {UnsavedWorldsCount} unsaved worlds", unsavedWorlds.Count);
        foreach (var world in unsavedWorlds)
        {
            _logger.LogInformation("Asking user ");
            //show mudblazor dialog asking if user wants to save unsaved worlds
            var parameters = new DialogParameters
            {
                { nameof(UnsavedWorldDialog.WorldName), world.Name }
            };
            var options = new DialogOptions
            {
                CloseButton = true,
                CloseOnEscapeKey = true,
                DisableBackdropClick = true
            };
            var dialog = await _dialogService.ShowAsync<UnsavedWorldDialog>("Unsaved changes!", parameters, options);
            var result = await dialog.Result;
            if (result.Canceled)
            {
                args.CancelShutdown();
                return;
            }

            if (result.Data is not bool) throw new ApplicationException("Unexpected dialog result type");
            if(result.Data is true) await SaveLearningWorldAsync(world);
        }
    }

    private static bool WorldHasUnsavedChanges(LearningWorldViewModel world)
    {
        return world.UnsavedChanges;
    }

    #endregion
}