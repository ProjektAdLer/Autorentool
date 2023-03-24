using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// The AuthoringToolWorkspacePresenter is the central component that controls changes to the <see cref="AuthoringToolWorkspaceViewModel"/>.
/// </summary>
public class AuthoringToolWorkspacePresenter : IAuthoringToolWorkspacePresenter,
    IAuthoringToolWorkspacePresenterToolboxInterface
{
    public AuthoringToolWorkspacePresenter(IAuthoringToolWorkspaceViewModel authoringToolWorkspaceVm,
        IPresentationLogic presentationLogic, ILearningSpacePresenter learningSpacePresenter,
        ILogger<AuthoringToolWorkspacePresenter> logger, IShutdownManager shutdownManager)
    {
        _learningSpacePresenter = learningSpacePresenter;
        AuthoringToolWorkspaceVm = authoringToolWorkspaceVm;
        _presentationLogic = presentationLogic;
        _logger = logger;
        _shutdownManager = shutdownManager;
        DeletedUnsavedWorld = null;
        InformationMessageToShow = null;
        if (presentationLogic.RunningElectron)
            //register callback so we can check for unsaved data on quit
            //TODO: register to our own quit button
            shutdownManager.BeforeShutdown += OnBeforeShutdown;
    }

    public IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }

    private readonly IPresentationLogic _presentationLogic;
    private readonly ILearningSpacePresenter _learningSpacePresenter;
    private readonly ILogger<AuthoringToolWorkspacePresenter> _logger;
    private readonly IShutdownManager _shutdownManager;

    public bool LearningWorldSelected => AuthoringToolWorkspaceVm.SelectedLearningWorld != null;

    public Queue<LearningWorldViewModel>? UnsavedWorldsQueue { get; set; }
    public LearningWorldViewModel? DeletedUnsavedWorld { get; set; }
    public string? InformationMessageToShow { get; set; }

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
        OnLearningWorldCreate?.Invoke(this, AuthoringToolWorkspaceVm.SelectedLearningWorld);
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
        if (AuthoringToolWorkspaceVm.SelectedLearningWorld == null)
            throw new ApplicationException("SelectedLearningWorld is null");
        await SaveLearningWorldAsync(AuthoringToolWorkspaceVm.SelectedLearningWorld);
    }


    internal void OnBeforeShutdown(object? _, BeforeShutdownEventArgs args)
    {
        //TODO: Need to redo the dialog and save queue
    }

    #endregion
}