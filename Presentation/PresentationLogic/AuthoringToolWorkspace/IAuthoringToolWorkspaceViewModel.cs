using System.ComponentModel;
using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// Interface for the ViewModel for the AuthoringToolWorkspace component.
/// </summary>
public interface IAuthoringToolWorkspaceViewModel : INotifyPropertyChanged, IWorldNamesProvider
{
    /// <summary>
    /// Read-only list of the Worlds contained in the Workspace ViewModel.
    /// </summary>
    IList<WorldViewModel> Worlds { get; }

    
    /// <summary>
    /// Removes a <see cref="WorldViewModel"/> from the Workspace ViewModel.
    /// </summary>
    /// <param name="world">The world that shall be removed.</param>
    void RemoveWorld(WorldViewModel world);
    
    /// <summary>
    /// The currently selected world.
    /// </summary>
    /// <exception cref="ArgumentException">The world to be set isn't contained in the collection.</exception>
    WorldViewModel? SelectedWorld { get; set; }
    
    /// <summary>
    /// A dictionary containing the initial values for fields in the 'Edit World' dialog.
    /// </summary>
    IDictionary<string, string>? EditDialogInitialValues { get; set; }
}