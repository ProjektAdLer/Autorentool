using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspacePresenter
{
    IAuthoringToolWorkspaceViewModel AuthoringToolWorkspaceVm { get; }

    /// <summary>
    /// Creates a new <see cref="LearningWorldViewModel"/> in the <see cref="AuthoringToolWorkspaceViewModel"/>
    /// </summary>
    /// <param name="name">Name of the new learning world</param>
    /// <param name="shortname">Shortname of the new learning world</param>
    /// <param name="authors">Authors of the new learning world</param>
    /// <param name="language">Language of the new learning world</param>
    /// <param name="description">Description of the new learning world</param>
    /// <param name="goals">Goals of the new learning world</param>
    /// <param name="evaluationLink">Link to the evaluation displayed on completion.</param>
    /// <param name="enrolmentKey">Key for users to enrol in the learning world.</param>
    /// <param name="storyStart"></param>
    /// <param name="storyEnd"></param>
    void CreateLearningWorld(string name, string shortname, string authors, string language, string description,
        string goals, string evaluationLink, string enrolmentKey, string storyStart, string storyEnd);

    /// <summary>
    /// Asks the user for confirmation and asks for saving the learning world if it was changed. Then deletes the
    /// learning world from the workspace view model.
    /// </summary>
    /// <param name="learningWorld">The learning world that should be deleted.</param>
    /// <exception cref="ApplicationException">Thrown if a dialog returns a invalid result. This should not happen.</exception>
    Task DeleteLearningWorld(ILearningWorldViewModel learningWorld);
}