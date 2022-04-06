namespace AuthoringTool.PresentationLogic.LearningElement;

internal class LearningElementPresenter : ILearningElementPresenter
{
    public LearningElementViewModel CreateNewLearningElement(string name, string shortname,
        ILearningElementViewModelParent parent,
        string type, string content, string authors, string description, string goals, double posx = 0f,
        double posy = 0f)
    {
        return new LearningElementViewModel(name, shortname, parent, type, content, authors,
            description, goals, posx, posy);
    }

    public LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        ILearningElementViewModelParent parent, string type, string content, string authors, string description, string goals, double? posx = null,
        double? posy = null)
    {
        element.Name = name;
        element.Shortname = shortname;
        element.Parent = parent;
        element.Type = type;
        element.Content = content;
        element.Authors = authors;
        element.Description = description;
        element.Goals = goals;
        element.PositionX = posx ?? element.PositionX;
        element.PositionY = posy ?? element.PositionY;
        return element;
    }
}