using MudBlazor;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// The AuthoringToolWorkspacePresenter is the central component that controls changes to the <see cref="AuthoringToolWorkspaceViewModel"/>.
/// </summary>
public class AuthoringToolWorkspacePresenter : IAuthoringToolWorkspacePresenter,
    IAuthoringToolWorkspacePresenterToolboxInterface
{
    public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IPresentationLogic presentationLogic, ILearningSpacePresenter learningSpacePresenter,
        ILogger<AuthoringToolWorkspacePresenter> logger, ISelectedViewModelsProvider selectedViewModelsProvider, IShutdownManager shutdownManager,
        IDialogService dialogService)
    {
        _learningSpacePresenter = learningSpacePresenter;
        AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _shutdownManager = shutdownManager;
        _dialogService = dialogService;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        if (presentationLogic.RunningElectron)
            //register callback so we can check for unsaved data on quit
            //TODO: register to our own quit button
            shutdownManager.BeforeShutdown += OnBeforeShutdownAsync;
    }

    public IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;
    private readonly IShutdownManager _shutdownManager;
    private readonly IDialogService _dialogService;

    public bool LearningWorldSelected => _selectedViewModelsProvider.LearningWorld != null;

    public event Action? OnForceViewUpdate;


    #region LearningWorld

    public void CreateLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals)
    {
        _presentationLogic.CreateLearningWorld(AuthoringToolWorkspaceVm, name, shortname, authors, language, description, goals);
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
        _selectedViewModelsProvider.SetLearningWorld(world, null);
    }

    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="learningWorld">The learning world that should be set as selected</param>
    internal void SetSelectedLearningWorld(LearningWorldViewModel? learningWorld)
    {
        _selectedViewModelsProvider.SetLearningWorld(learningWorld, null);
    }

    /// <summary>
    /// Deletes the currently selected learning world from the view model and selects the last learning world in the
    /// collection, if any remain.
    /// </summary>
    public void DeleteSelectedLearningWorld()
    {
        var learningWorld = _selectedViewModelsProvider.LearningWorld;
        if (learningWorld == null) return;
        _presentationLogic.DeleteLearningWorld(AuthoringToolWorkspaceVm, learningWorld);
    }

    public void AddLearningWorld(LearningWorldViewModel learningWorld)
    {
        _presentationLogic.AddLearningWorld(AuthoringToolWorkspaceVm, learningWorld);
    }

    public async Task LoadLearningWorldAsync()
    {
        await _presentationLogic.LoadLearningWorldAsync(AuthoringToolWorkspaceVm);
    }

    internal async Task SaveLearningWorldAsync(ILearningWorldViewModel world)
    {
        await _presentationLogic.SaveLearningWorldAsync(world);
    }

    public async Task SaveSelectedLearningWorldAsync()
    {
        if (_selectedViewModelsProvider.LearningWorld == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        await SaveLearningWorldAsync(_selectedViewModelsProvider.LearningWorld);
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

    private static bool WorldHasUnsavedChanges(ILearningWorldViewModel world)
    {
        return world.UnsavedChanges;
    }

    #endregion
}