using BusinessLogic.Entities;
using ElementTypeEnum = Shared.ElementTypeEnum;
using ContentTypeEnum = Shared.ContentTypeEnum;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands;

public class CreateLearningElement : IUndoCommand
{
    private readonly ILearningElementParent _elementParent;
    private readonly LearningElement _learningElement;
    private readonly Action<ILearningElementParent> _mappingAction;
    private IMemento? _memento;

    public CreateLearningElement(ILearningElementParent elementParent, string name, string shortName,
        ElementTypeEnum elementType, ContentTypeEnum contentType, LearningContent learningContent, string authors,
        string description, string goals, LearningElementDifficultyEnum difficulty, int workload, int points,
        Action<ILearningElementParent> mappingAction)
    {
        _learningElement = elementType switch
        {
            ElementTypeEnum.Transfer => CreateNewTransferElement(name, shortName, elementParent, contentType,
                learningContent, authors, description, goals, difficulty, workload, points),
            ElementTypeEnum.Activation => CreateNewActivationElement(name, shortName, elementParent, contentType,
                learningContent, authors, description, goals, difficulty, workload, points),
            ElementTypeEnum.Interaction => CreateNewInteractionElement(name, shortName, elementParent, contentType,
                learningContent, authors, description, goals, difficulty, workload, points),
            ElementTypeEnum.Test => CreateNewTestElement(name, shortName, elementParent, contentType,
                learningContent, authors, description, goals, difficulty, workload, points),
            _ => throw new ApplicationException("no valid ElementType assigned")
        };
        _elementParent = elementParent;
        _mappingAction = mappingAction;
    }
    
    public CreateLearningElement(ILearningElementParent elementParent, LearningElement learningElement,
        Action<ILearningElementParent> mappingAction)
    {
        _learningElement = learningElement;
        _elementParent = elementParent;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = _elementParent.GetMemento();

        _elementParent.LearningElements.Add(_learningElement);
        
        _mappingAction.Invoke(_elementParent);
    }

    private LearningElement CreateNewTransferElement(string name, string shortname, ILearningElementParent parent,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.Image => new ImageTransferElement(name, shortname, parent, learningContent,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.Video => new VideoTransferElement(name, shortname, parent, learningContent,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.Pdf => new PdfTransferElement(name, shortname, parent, learningContent, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private LearningElement CreateNewActivationElement(string name, string shortname, ILearningElementParent parent,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.Video => new VideoActivationElement(name, shortname, parent, learningContent,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.H5P => new H5PActivationElement(name, shortname, parent, learningContent, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private LearningElement CreateNewInteractionElement(string name, string shortname, ILearningElementParent parent,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PInteractionElement(name, shortname, parent, learningContent, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private LearningElement CreateNewTestElement(string name, string shortname, ILearningElementParent parent,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PTestElement(name, shortname, parent, learningContent, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        _elementParent.RestoreMemento(_memento);
        
        _mappingAction.Invoke(_elementParent);
    }

    public void Redo() => Execute();
}