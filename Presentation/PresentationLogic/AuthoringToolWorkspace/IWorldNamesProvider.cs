namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public interface IWorldNamesProvider
{
    /// <summary>
    /// Returns the names of all worlds in the provider.
    /// </summary>
    public IEnumerable<string> WorldNames { get; }
    /// <summary>
    /// Returns the short names of all worlds in the provider.
    /// </summary>
    public IEnumerable<string> WorldShortNames { get; }
}