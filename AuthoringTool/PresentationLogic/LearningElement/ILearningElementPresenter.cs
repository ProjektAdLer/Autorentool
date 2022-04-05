namespace AuthoringTool.PresentationLogic.LearningElement;

public interface ILearningElementPresenter
{
    LearningElementViewModel CreateNewLearningElement(string name, string shortname, string parent, string assignment,
        string type, string content, string authors, string description, string goals);

    LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        string parent, string assignment, string type, string content, string authors, string description, string goals);
}