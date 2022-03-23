namespace AuthoringTool.PresentationLogic.LearningElement;

public interface ILearningElementPresenter
{
    LearningElementViewModel CreateNewLearningElement(string name, string shortname, string content, string authors,
        string description, string goals);

    LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        string content, string authors, string description, string goals);
}