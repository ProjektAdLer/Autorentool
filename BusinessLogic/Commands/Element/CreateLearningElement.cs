using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using ElementTypeEnum = Shared.ElementTypeEnum;
using ContentTypeEnum = Shared.ContentTypeEnum;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands.Element;

public class CreateLearningElement : IUndoCommand
{
    internal LearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement LearningElement { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public CreateLearningElement(LearningSpace parentSpace, int slotIndex, string name, string shortName,
        ElementTypeEnum elementType, ContentTypeEnum contentType, LearningContent learningContent, 
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload, 
        int points, double positionX, double positionY, Action<LearningSpace> mappingAction)
    {
        LearningElement = elementType switch
        {
            ElementTypeEnum.Transfer => CreateNewTransferElement(name, shortName, parentSpace, contentType,
                learningContent, authors, description, goals, difficulty, workload, points, positionX, positionY),
            ElementTypeEnum.Activation => CreateNewActivationElement(name, shortName, parentSpace, contentType,
                learningContent, authors, description, goals, difficulty, workload, points, positionX, positionY),
            ElementTypeEnum.Interaction => CreateNewInteractionElement(name, shortName, parentSpace, contentType,
                learningContent, authors, description, goals, difficulty, workload, points, positionX, positionY),
            ElementTypeEnum.Test => CreateNewTestElement(name, shortName, parentSpace, contentType,
                learningContent, authors, description, goals, difficulty, workload, points, positionX, positionY),
            _ => throw new ApplicationException("no valid ElementType assigned")
        };
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _mappingAction = mappingAction;
    }
    
    public CreateLearningElement(LearningSpace parentSpace, int slotIndex, LearningElement learningElement,
        Action<LearningSpace> mappingAction)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();

        ParentSpace.LearningSpaceLayout.LearningElements[SlotIndex] = LearningElement;
        ParentSpace.SelectedLearningElement = LearningElement;
        
        _mappingAction.Invoke(ParentSpace);
    }

    private LearningElement CreateNewTransferElement(string name, string shortname, LearningSpace parentSpace,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.Image => new ImageTransferElement(name, shortname, parentSpace, learningContent,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.Video => new VideoTransferElement(name, shortname, parentSpace, learningContent,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.PDF => new PdfTransferElement(name, shortname, parentSpace, learningContent, authors,
                description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.Text => new TextTransferElement(name, shortname, parentSpace, learningContent, authors,
            description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private LearningElement CreateNewActivationElement(string name, string shortname, LearningSpace parentSpace,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.Video => new VideoActivationElement(name, shortname, parentSpace, learningContent,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.H5P => new H5PActivationElement(name, shortname, parentSpace, learningContent, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private LearningElement CreateNewInteractionElement(string name, string shortname, LearningSpace parentSpace,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PInteractionElement(name, shortname, parentSpace, learningContent, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private LearningElement CreateNewTestElement(string name, string shortname, LearningSpace parentSpace,
        ContentTypeEnum contentType, LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        LearningElement element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PTestElement(name, shortname, parentSpace, learningContent, authors,
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
        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }
        
        ParentSpace.RestoreMemento(_memento);
        ParentSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);
        
        _mappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}