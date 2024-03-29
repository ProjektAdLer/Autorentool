using System.ComponentModel;
using BusinessLogic.Validation;
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
    IList<ILearningWorldViewModel> LearningWorlds { get; }

    
    /// <summary>
    /// Removes a <see cref="LearningWorldViewModel"/> from the Workspace ViewModel.
    /// </summary>
    /// <param name="learningWorld">The learning world that shall be removed.</param>
    void RemoveLearningWorld(LearningWorldViewModel learningWorld);
}