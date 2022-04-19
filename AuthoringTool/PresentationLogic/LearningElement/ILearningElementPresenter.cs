using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public interface ILearningElementPresenter
{
    LearningElementViewModel CreateNewLearningElement(string name, string shortname,
        ILearningElementViewModelParent parent, string elementType, string contentType, string authors, string description,
        string goals, double posx = 0f, double posy = 0f,LearningContentViewModel? learningContent = null);

    LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        ILearningElementViewModelParent parent, string elementType, string contentType, string authors, string description,
        string goals, double? posx = null, double? posy = null);

    void RemoveLearningElementFromParentAssignment(LearningElementViewModel element);
}