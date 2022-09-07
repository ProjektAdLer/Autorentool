using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.LearningElement;

public class LearningElementPresenter : ILearningElementPresenter
{
    /// <summary>
    /// Removes assignment of a learning element to its parent
    /// </summary>
    /// <param name="element">Element, that gets removed from its parent</param>
    public void RemoveLearningElementFromParentAssignment(LearningElementViewModel element)
    {
        switch (element.Parent)
        {
            case null:
                break;
            case LearningWorldViewModel world:
                world.LearningElements.Remove(element);
                break;
            case LearningSpaceViewModel space:
                space.LearningElements.Remove(element);
                break;
        }
    }
}