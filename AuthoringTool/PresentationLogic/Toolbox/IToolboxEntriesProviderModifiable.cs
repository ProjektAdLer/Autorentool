using AuthoringTool.View.Toolbox;

namespace AuthoringTool.PresentationLogic.Toolbox;

/// <summary>
/// Provides all <see cref="IDisplayableLearningObject"/> objects that should be presented in the Toolbox.
/// Also allows to add new Entries.
/// </summary>
public interface IToolboxEntriesProviderModifiable : IToolboxEntriesProvider
{
    /// <summary>
    /// Adds a new entry to <see cref="IToolboxEntriesProvider.Entries"/>.
    /// </summary>
    /// <param name="obj">The entry that should be added.</param>
    /// <returns>Whether or not adding the entry was successful.</returns>
    /// <remarks>This method will fail when trying to add an object while an object of the same type and name already
    /// is contained in <see cref="IToolboxEntriesProvider.Entries"/></remarks>
    bool AddEntry(IDisplayableLearningObject obj);
}