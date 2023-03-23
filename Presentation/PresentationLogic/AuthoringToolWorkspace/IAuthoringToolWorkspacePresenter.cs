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
    /// Creates a new <see cref="LearningWorldViewModel"/> in the <see cref="AuthoringToolWorkspaceViewModel"/>
    /// </summary>
    /// <param name="name">Name of the new learning world</param>
    /// <param name="shortname">Shortname of the new learning world</param>
    /// <param name="authors">Authors of the new learning world</param>
    /// <param name="language">Language of the new learning world</param>
    /// <param name="description">Description of the new learning world</param>
    /// <param name="goals">Goals of the new learning world</param>
    void CreateLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals);

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