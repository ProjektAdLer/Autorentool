namespace Presentation.PresentationLogic.World;

public interface ISpaceNamesProvider
{
    
    /// <summary>
    /// Returns the names of all spaces in the current world selected in the provider.
    /// </summary>
    public IEnumerable<string>? SpaceNames { get; }
    /// <summary>
    /// Returns the short names of all spaces in the current world selected in the provider.
    /// </summary>
    public IEnumerable<string>? SpaceShortnames { get; }
}