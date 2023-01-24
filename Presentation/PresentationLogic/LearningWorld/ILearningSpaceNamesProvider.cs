namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningSpaceNamesProvider
{
    
    /// <summary>
    /// Returns the names of all spaces in the current world selected in the provider.
    /// </summary>
    public IEnumerable<string>? LearningSpaceNames { get; }
    /// <summary>
    /// Returns the short names of all spaces in the current world selected in the provider.
    /// </summary>
    public IEnumerable<string>? LearningSpaceShortnames { get; }
}