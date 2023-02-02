namespace Presentation.PresentationLogic;

/// <summary>
/// Read-only interface for components that need to display properties of DisplayableObjects
/// </summary>
public interface IDisplayableObject
{
    /// <summary>
    /// The name of the displayable object.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// The file ending that is associated with the type of displayable object.
    /// </summary>
    public string FileEnding { get; }
}