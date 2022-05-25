using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public interface ILearningElementPresenter
{
    LearningElementViewModel CreateNewLearningElement(string name, string shortname,
        ILearningElementViewModelParent parent, LearningContentViewModel learningContent, string authors, string description,
        string goals, double posx = 0f, double posy = 0f);

    LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        ILearningElementViewModelParent parent, string authors, string description,
        string goals, double? posx = null, double? posy = null);

    void RemoveLearningElementFromParentAssignment(LearningElementViewModel element);
}