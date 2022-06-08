using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.LearningElement;

internal class LearningElementPresenter : ILearningElementPresenter
{
    public LearningElementViewModel CreateNewTransferElement(string name, string shortname,
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, int workload, double posx = 0f, double posy = 0f)
    {
        LearningElementViewModel element = contentType switch
        {
            ContentTypeEnum.Image => new ImageTransferElementViewModel(name, shortname, parent, learningContent,
                authors, description, goals, workload, posx, posy),
            ContentTypeEnum.Video => new VideoTransferElementViewModel(name, shortname, parent, learningContent,
                authors, description, goals, workload, posx, posy),
            ContentTypeEnum.Pdf => new PdfTransferElementViewModel(name, shortname, parent, learningContent, authors,
                description, goals, workload, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        AddLearningElementParentAssignment(parent, element);

        return element;
    }

    public LearningElementViewModel CreateNewActivationElement(string name, string shortname,
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, int workload, double posx = 0f, double posy = 0f)
    {
        LearningElementViewModel element = contentType switch
        {
            ContentTypeEnum.Video => new VideoActivationElementViewModel(name, shortname, parent, learningContent, authors,
                description, goals, workload, posx, posy),
            ContentTypeEnum.H5P => new H5PActivationElementViewModel(name, shortname, parent, learningContent, authors,
                description, goals, workload, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        AddLearningElementParentAssignment(parent, element);

        return element;
    }

    public LearningElementViewModel CreateNewInteractionElement(string name, string shortname,
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, int workload, double posx = 0f, double posy = 0f)
    {
        LearningElementViewModel element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PInteractionElementViewModel(name, shortname, parent, learningContent, authors,
                description, goals, workload, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        AddLearningElementParentAssignment(parent, element);

        return element;
    }
    
    public LearningElementViewModel CreateNewTestElement(string name, string shortname,
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, int workload, double posx = 0f, double posy = 0f)
    {
        LearningElementViewModel element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PTestElementViewModel(name, shortname, parent, learningContent, authors, description,
                goals, workload, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
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
        ILearningElementViewModelParent parent, string authors, string description, string goals, int workload,
        double? posx = null, double? posy = null)
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
        element.Workload = workload;
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