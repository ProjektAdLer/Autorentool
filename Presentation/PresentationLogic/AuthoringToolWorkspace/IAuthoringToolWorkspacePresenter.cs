using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspacePresenter
{
    IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }
    bool LearningWorldSelected { get; }

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
    /// <summary>
    /// Asks the user for confirmation and asks for saving the learning world if it was changed. Then deletes the
    /// learning world from the workspace view model.
    /// </summary>
    /// <param name="learningWorld">The learning world that should be deleted.</param>
    /// <exception cref="ApplicationException">Thrown if a dialog returns a invalid result. This should not happen.</exception>
    Task DeleteLearningWorld(ILearningWorldViewModel learningWorld);
}