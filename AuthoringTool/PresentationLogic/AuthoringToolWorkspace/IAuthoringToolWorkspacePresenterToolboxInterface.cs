using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspacePresenterToolboxInterface
{
    /// <summary>
    /// Adds learning world to view model or, if there already exists a learning world with the same name in the view model,
    /// begins replacement process.
    /// </summary>
    /// <param name="learningWorld">The learning world that should be added.</param>
    void AddLearningWorld(LearningWorldViewModel learningWorld);
}