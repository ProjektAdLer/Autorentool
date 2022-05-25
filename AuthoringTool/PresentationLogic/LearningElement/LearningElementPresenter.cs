using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.LearningElement;

internal class LearningElementPresenter : ILearningElementPresenter
{
    public LearningElementViewModel CreateNewLearningElement(string name, string shortname,
        ILearningElementViewModelParent parent,LearningContentViewModel learningContent,
        string authors, string description, string goals, double posx = 0f, double posy = 0f)
    {
        var element = new LearningElementViewModel(name, shortname, parent, learningContent, authors,
            description, goals, posx, posy);

        AddLearningElementParentAssignment(parent, element);

        return element;
    }

    /// <summary>
    /// Adds the assignment of a learning element to a parent 
    /// </summary>
    /// <param name="parent">Parent of the learning element. Can either be a learning world or a learning space</param>
    /// <param name="element">Element that gets assigned to its parent</param>
    /// <exception cref="NotImplementedException">Thrown, when parent is neither a world or a space</exception>
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
        ILearningElementViewModelParent parent, string authors, string description, string goals, double? posx = null,
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
        element.Authors = authors;
        element.Description = description;
        element.Goals = goals;
        element.PositionX = posx ?? element.PositionX;
        element.PositionY = posy ?? element.PositionY;
        return element;
    }

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