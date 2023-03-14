using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspacePresenter
{
    IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }
    bool LearningWorldSelected { get; }
    LearningWorldViewModel? DeletedUnsavedWorld { get; set; }
    string? InformationMessageToShow { get; set; }
    Queue<LearningWorldViewModel>? UnsavedWorldsQueue { get; set; }

    event Action? OnForceViewUpdate;

    /// <summary>
    /// Sets the selected <see cref="LearningWorldViewModel"/> in the view model.
    /// </summary>
    /// <param name="worldName">The name of the world that should be selected.</param>
    /// <exception cref="ArgumentException">Thrown when no world with that name is registered in the view model.</exception>
    void SetSelectedLearningWorld(string worldName);

    /// <summary>
    /// Deletes the currently selected learning world from the view model and selects the last learning world in the
    /// collection, if any remain.
    /// </summary>
    void DeleteSelectedLearningWorld();
    Task LoadLearningWorldAsync();
    Task SaveSelectedLearningWorldAsync();
}