namespace AuthoringTool.PresentationLogic.LearningElement;

internal class LearningElementPresenter : ILearningElementPresenter
{
    public LearningElementViewModel CreateNewLearningElement(string name, string shortname, string type, string content,
        string authors, string description, string goals)
    {
        return new LearningElementViewModel(name, shortname, type, content, authors,
            description, goals);
    }

    public LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        string type, string content, string authors, string description, string goals)
    {
        element.Name = name;
        element.Shortname = shortname;
        element.Type = type;
        element.Content = content;
        element.Authors = authors;
        element.Description = description;
        element.Goals = goals;
        return element;
    }
}