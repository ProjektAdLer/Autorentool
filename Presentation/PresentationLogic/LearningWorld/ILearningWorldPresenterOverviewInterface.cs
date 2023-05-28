using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenterOverviewInterface
{
    ILearningWorldViewModel LearningWorldVm { get; }

    /// <summary>
    /// Sets the selected learning element of the learning world to the given learning element.
    /// </summary>
    /// <param name="learningElement">The learning element to set.</param>
    /// <exception cref="ApplicationException">Thrown if no learning world is currently selected.</exception>
    void SetSelectedLearningElement(ILearningElementViewModel learningElement);

    void SetSelectedLearningSpace(IObjectInPathWayViewModel obj);
}