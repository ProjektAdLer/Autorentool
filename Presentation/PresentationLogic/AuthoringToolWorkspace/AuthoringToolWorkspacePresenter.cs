using System.Runtime.Serialization;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// The AuthoringToolWorkspacePresenter is the central component that controls changes to the <see cref="AuthoringToolWorkspaceViewModel"/>.
/// </summary>
public class AuthoringToolWorkspacePresenter : IAuthoringToolWorkspacePresenter, IDisposable, IAsyncDisposable
{
    private readonly IDialogService _dialogService;
    private readonly IErrorService _errorService;
    private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;

    private readonly IPresentationLogic _presentationLogic;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;
    private readonly IShutdownManager _shutdownManager;
    private readonly IStringLocalizer<AuthoringToolWorkspacePresenter> _localizer;

    public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IPresentationLogic presentationLogic, ILogger<AuthoringToolWorkspacePresenter> logger,
        ISelectedViewModelsProvider selectedViewModelsProvider, IShutdownManager shutdownManager,
        IDialogService dialogService, IErrorService errorService, IStringLocalizer<AuthoringToolWorkspacePresenter> localizer)
    {
        AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _shutdownManager = shutdownManager;
        _dialogService = dialogService;
        _errorService = errorService;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        _localizer = localizer;
        if (presentationLogic.RunningElectron)
            //register callback so we can check for unsaved data on quit
            shutdownManager.BeforeShutdown += OnBeforeShutdownAsync;
    }

    public bool LearningWorldSelected => _selectedViewModelsProvider.LearningWorld != null;

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    public IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }


    public void Dispose()
    {
        _shutdownManager.BeforeShutdown -= OnBeforeShutdownAsync;
    }

    private void LogAndSetError(string operation, string errorDetail, string? userMessage)
    {
        _logger.LogError("Error in {Operation}: {ErrorDetail}", operation, errorDetail);
        userMessage ??= errorDetail;
        _errorService.SetError("Operation failed", userMessage);
    }


    #region LearningWorld

    /// <inheritdoc cref="IAuthoringToolWorkspacePresenter.CreateLearningWorld"/>
    public void CreateLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, string evaluationLink, string enrolmentKey)
    {
        _presentationLogic.CreateLearningWorld(AuthoringToolWorkspaceVm, name, shortname, authors, language,
            description, goals, evaluationLink, enrolmentKey);
    }

    /// <inheritdoc cref="IAuthoringToolWorkspacePresenter.DeleteLearningWorld"/>
    public async Task DeleteLearningWorld(ILearningWorldViewModel learningWorld)
    {
        if (WorldHasUnsavedChanges(learningWorld))
        {
            var result = await AskForSaveWorld(learningWorld);
            if (result.Canceled) return;
            if (result.Data is not bool)
            {
                LogAndSetError("DeleteLearningWorld",
                    $"Unexpected dialog result type while trying to save world; expected bool, got {result.Data?.GetType()}",
                    "Unexpected dialog result type");
                return;
            }

            if (result.Data is true) SaveLearningWorld(learningWorld);
        }

        _presentationLogic.DeleteLearningWorld(AuthoringToolWorkspaceVm, learningWorld);
    }

    internal void SaveLearningWorld(ILearningWorldViewModel world)
    {
        try
        {
            _presentationLogic.SaveLearningWorld(world);
        }
        catch (SerializationException e)
        {
            _errorService.SetError("Error while saving world", e.Message);
        }
        catch (InvalidOperationException e)
        {
            _errorService.SetError("Error while saving world", e.Message);
        }
    }


    internal async Task OnBeforeShutdownAsync(object? sender, BeforeShutdownEventArgs args)
    {
        var unsavedWorlds = AuthoringToolWorkspaceVm.LearningWorlds.Where(WorldHasUnsavedChanges).ToList();
        _logger.LogInformation("Found {UnsavedWorldsCount} unsaved worlds", unsavedWorlds.Count);
        foreach (var world in unsavedWorlds)
        {
            var result = await AskForSaveWorld(world);
            if (result.Canceled)
            {
                args.CancelShutdown();
                return;
            }

            if (result.Data is not bool)
            {
                LogAndSetError("OnBeforeShutdownAsync",
                    $"Unexpected dialog result type while trying to save world; expected bool, got {result.Data?.GetType()}",
                    "Unexpected dialog result type");
                return;
            }

            if (result.Data is true) SaveLearningWorld(world);
        }
    }

    private async Task<DialogResult> AskForSaveWorld(ILearningWorldViewModel world)
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
            BackdropClick = false
        };
        var dialog = await _dialogService.ShowAsync<UnsavedWorldDialog>(_localizer["AuthoringToolWorkspacePresenter.SaveWorld.UnsavedChanges"], parameters, options);
        var result = await dialog.Result;
        if (result == null) return DialogResult.Cancel();
        return result;
    }

    private static bool WorldHasUnsavedChanges(ILearningWorldViewModel world)
    {
        return world.UnsavedChanges;
    }

    #endregion
}