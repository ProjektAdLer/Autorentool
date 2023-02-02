using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IAuthoringToolWorkspacePresenterToolboxInterface
{
    /// <summary>
    /// Adds world to view model or, if there already exists a world with the same name in the view model,
    /// begins replacement process.
    /// </summary>
    /// <param name="world">The world that should be added.</param>
    void AddWorld(WorldViewModel world);
}