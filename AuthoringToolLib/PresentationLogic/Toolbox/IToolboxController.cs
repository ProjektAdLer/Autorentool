namespace AuthoringToolLib.PresentationLogic.Toolbox;

/// <summary>
/// Controller for the Toolbox View Component
/// </summary>
public interface IToolboxController
{
    /// <summary>
    /// Load passed object into the AuthoringToolWorkspace correctly. Worlds will be loaded as an opened world,
    /// spaces will be loaded into the currently focused world and elements will be loaded either into the currently
    /// focused worlds or, if applicable, into the currently opened space.
    /// </summary>
    /// <param name="obj">The object to be loaded.</param>
    void LoadObjectIntoWorkspace(IDisplayableLearningObject obj);
}