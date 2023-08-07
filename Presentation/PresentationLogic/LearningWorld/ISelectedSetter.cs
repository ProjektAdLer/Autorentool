using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ISelectedSetter
{
    /// <summary>
    /// Sets the selected learning element of the learning world to the given learning element.
    /// </summary>
    /// <param name="learningElement">The learning element to set.</param>
    void SetSelectedLearningElement(ILearningElementViewModel learningElement);

    /// <summary>
    /// Sets the selected learning space and requests to open the space dialog.
    /// </summary>
    /// <param name="objectInPathWayViewModel">The object in the pathway to be selected as the learning space.</param>
    void SetSelectedLearningSpace(IObjectInPathWayViewModel objectInPathWayViewModel);
}