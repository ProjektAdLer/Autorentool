using System.ComponentModel;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

/// <summary>
/// Interface for the ViewModel for the AuthoringToolWorkspace component.
/// </summary>
public interface IAuthoringToolWorkspaceViewModel : INotifyPropertyChanged, ILearningWorldNamesProvider
{
    /// <summary>
    /// Read-only list of the LearningWorlds contained in the Workspace ViewModel.
    /// </summary>
    IList<LearningWorldViewModel> LearningWorlds { get; }

    
    /// <summary>
    /// Removes a <see cref="LearningWorldViewModel"/> from the Workspace ViewModel.
    /// </summary>
    /// <param name="learningWorld">The learning world that shall be removed.</param>
    void RemoveLearningWorld(LearningWorldViewModel learningWorld);
    
    /// <summary>
    /// The currently selected learning world.
    /// </summary>
    /// <exception cref="ArgumentException">The world to be set isn't contained in the collection.</exception>
    LearningWorldViewModel? SelectedLearningWorld { get; set; }
    
    /// <summary>
    /// A dictionary containing the initial values for fields in the 'Edit World' dialog.
    /// </summary>
    IDictionary<string, string>? EditDialogInitialValues { get; set; }
}