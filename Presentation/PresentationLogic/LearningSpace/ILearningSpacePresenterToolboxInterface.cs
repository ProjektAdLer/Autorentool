using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpacePresenterToolboxInterface
{
    void AddLearningElement(ILearningElementViewModel element, int slotIndex);
}