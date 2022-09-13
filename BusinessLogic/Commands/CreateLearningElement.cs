using BusinessLogic.Entities;
using ElementTypeEnum = Shared.ElementTypeEnum;
using ContentTypeEnum = Shared.ContentTypeEnum;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands;

public class CreateLearningElement : IUndoCommand
{
    private readonly ILearningElementParent _elementParent;
    private readonly string _name;
    private readonly string _shortName;
    private readonly ElementTypeEnum _elementType;
    private readonly ContentTypeEnum _contentType;
    private readonly LearningContent _learningContent;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly LearningElementDifficultyEnum _difficulty;
    private readonly int _workload;
    private readonly int _points;
    private readonly Action<ILearningElementParent> _mappingAction;
    private IMemento? _memento;

    public CreateLearningElement(ILearningElementParent elementParent, string name, string shortName,
        ElementTypeEnum elementType, ContentTypeEnum contentType, LearningContent learningContent, string authors,
        string description, string goals, LearningElementDifficultyEnum difficulty, int workload, int points,
        Action<ILearningElementParent> mappingAction)
    {
        _elementParent = elementParent;
        _name = name;
        _shortName = shortName;
        _elementType = elementType;
        _contentType = contentType;
        _learningContent = learningContent;
        _authors = authors;
        _description = description;
        _goals = goals;
        _difficulty = difficulty;
        _workload = workload;
        _points = points;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = _elementParent.GetMemento();
        
        var learningElement = _elementType switch
        {
            ElementTypeEnum.Transfer => CreateNewTransferElement(_name, _shortName, _elementParent, _contentType,
                _learningContent, _authors, _description, _goals, _difficulty, _workload, _points),
            ElementTypeEnum.Activation => CreateNewActivationElement(_name, _shortName, _elementParent, _contentType,
                _learningContent, _authors, _description, _goals, _difficulty, _workload, _points),
            ElementTypeEnum.Interaction => CreateNewInteractionElement(_name, _shortName, _elementParent, _contentType,
                _learningContent, _authors, _description, _goals, _difficulty, _workload, _points),
            ElementTypeEnum.Test => CreateNewTestElement(_name, _shortName, _elementParent, _contentType,
                _learningContent, _authors, _description, _goals, _difficulty, _workload, _points),
            _ => throw new ApplicationException("no valid ElementType assigned")
        };
        
        _elementParent.LearningElements.Add(learningElement);
        
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