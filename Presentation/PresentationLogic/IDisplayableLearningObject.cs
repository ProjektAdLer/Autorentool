namespace Presentation.PresentationLogic;

/// <summary>
/// Read-only interface for components that need to display properties of LearningObjects
/// </summary>
public interface IDisplayableLearningObject
{
    /// <summary>
    /// The name of the learning object.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// The file ending that is associated with the type of learning object.
    /// </summary>
    public string FileEnding { get; }
}