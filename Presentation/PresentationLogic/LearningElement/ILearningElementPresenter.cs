using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.PresentationLogic.LearningElement;

public interface ILearningElementPresenter
{
    LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        ILearningElementViewModelParent parent, string authors, string description,
        string goals, LearningElementDifficultyEnum difficulty, int workload, double? posx = null, double? posy = null);
    void RemoveLearningElementFromParentAssignment(LearningElementViewModel element);
}