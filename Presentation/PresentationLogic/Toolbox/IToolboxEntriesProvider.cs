namespace Presentation.PresentationLogic.Toolbox;

/// <summary>
/// Provides all <see cref="IDisplayableObject"/> objects that should be presented in the Toolbox.
/// If you need to modify this collection, use <see cref="IToolboxEntriesProviderModifiable"/> instead.
/// </summary>
public interface IToolboxEntriesProvider
{
    /// <summary>
    /// Provides all entries for the Toolbox.
    /// </summary>
    IEnumerable<IDisplayableObject> Entries { get; }
}