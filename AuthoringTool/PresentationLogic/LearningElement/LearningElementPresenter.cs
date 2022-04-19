using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.LearningElement;

internal class LearningElementPresenter : ILearningElementPresenter
{
    public LearningElementViewModel CreateNewLearningElement(string name, string shortname,
        ILearningElementViewModelParent parent, string elementType, string contentType,
        string authors, string description, string goals, double posx = 0f, double posy = 0f, LearningContentViewModel? learningContent = null)
    {
        var element = new LearningElementViewModel(name, shortname, parent, elementType, contentType, learningContent, authors,
            description, goals, posx, posy);

        AddLearningElementParentAssignment(parent, element);

        return element;
    }

    private static void AddLearningElementParentAssignment(ILearningElementViewModelParent parent,
        LearningElementViewModel element)
    {
        switch (parent)
        {
            case LearningWorldViewModel world:
                world.LearningElements.Add(element);
                break;
            case LearningSpaceViewModel space:
                space.LearningElements.Add(element);
                break;
            default:
                throw new NotImplementedException("Type of Assignment is not implemented");
        }
    }

    public LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        ILearningElementViewModelParent parent, string elementType, string contentType, string authors, string description, string goals, double? posx = null,
        double? posy = null)
    {
        if (parent.Name != element.Parent?.Name)
        {
            RemoveLearningElementFromParentAssignment(element);
            AddLearningElementParentAssignment(parent, element);
        }
        
        element.Name = name;
        element.Shortname = shortname;
        element.Parent = parent;
        element.ElementType = elementType;
        element.ContentType = contentType;
        element.Authors = authors;
        element.Description = description;
        element.Goals = goals;
        element.PositionX = posx ?? element.PositionX;
        element.PositionY = posy ?? element.PositionY;
        return element;
    }

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