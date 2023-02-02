using BusinessLogic.Entities;
using Shared;
using ElementTypeEnum = Shared.ElementTypeEnum;
using ContentTypeEnum = Shared.ContentTypeEnum;

namespace BusinessLogic.Commands.Element;

public class CreateElement : IUndoCommand
{
    internal Entities.Space ParentSpace { get; }
    internal int SlotIndex { get; }
    internal Entities.Element Element { get; }
    private readonly Action<Entities.Space> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public CreateElement(Entities.Space parentSpace, int slotIndex, string name, string shortName,
        ElementTypeEnum elementType, ContentTypeEnum contentType, Content content, string url, 
        string authors, string description, string goals, ElementDifficultyEnum difficulty, int workload, 
        int points, double positionX, double positionY, Action<Entities.Space> mappingAction)
    {
        Element = elementType switch
        {
            ElementTypeEnum.Transfer => CreateNewTransferElement(name, shortName, parentSpace, contentType,
                content, url, authors, description, goals, difficulty, workload, points, positionX, positionY),
            ElementTypeEnum.Activation => CreateNewActivationElement(name, shortName, parentSpace, contentType,
                content, url, authors, description, goals, difficulty, workload, points, positionX, positionY),
            ElementTypeEnum.Interaction => CreateNewInteractionElement(name, shortName, parentSpace, contentType,
                content, url, authors, description, goals, difficulty, workload, points, positionX, positionY),
            ElementTypeEnum.Test => CreateNewTestElement(name, shortName, parentSpace, contentType,
                content, url, authors, description, goals, difficulty, workload, points, positionX, positionY),
            _ => throw new ApplicationException("no valid ElementType assigned")
        };
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _mappingAction = mappingAction;
    }
    
    public CreateElement(Entities.Space parentSpace, int slotIndex, Entities.Element element,
        Action<Entities.Space> mappingAction)
    {
        Element = element;
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.SpaceLayout.GetMemento();

        ParentSpace.SpaceLayout.Elements[SlotIndex] = Element;
        ParentSpace.SelectedElement = Element;
        
        _mappingAction.Invoke(ParentSpace);
    }

    private Entities.Element CreateNewTransferElement(string name, string shortname, Entities.Space parentSpace,
        ContentTypeEnum contentType, Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        Entities.Element element = contentType switch
        {
            ContentTypeEnum.Image => new ImageTransferElement(name, shortname, parentSpace, content, url,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.Video => new VideoTransferElement(name, shortname, parentSpace, content, url,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.PDF => new PdfTransferElement(name, shortname, parentSpace, content, url, authors,
                description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.Text => new TextTransferElement(name, shortname, parentSpace, content, url, authors,
            description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private Entities.Element CreateNewActivationElement(string name, string shortname, Entities.Space parentSpace,
        ContentTypeEnum contentType, Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        Entities.Element element = contentType switch
        {
            ContentTypeEnum.Video => new VideoActivationElement(name, shortname, parentSpace, content, url,
                authors, description, goals, difficulty, workload, points, posx, posy),
            ContentTypeEnum.H5P => new H5PActivationElement(name, shortname, parentSpace, content, url, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private Entities.Element CreateNewInteractionElement(string name, string shortname, Entities.Space parentSpace,
        ContentTypeEnum contentType, Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        Entities.Element element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PInteractionElement(name, shortname, parentSpace, content, url, authors,
                description, goals, difficulty, workload, points, posx, posy),
            _ => throw new ApplicationException("No Valid ContentType assigned")
        };
        return element;
    }
    
    private Entities.Element CreateNewTestElement(string name, string shortname, Entities.Space parentSpace,
        ContentTypeEnum contentType, Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double posx = 0f, double posy = 0f)
    {
        Entities.Element element = contentType switch
        {
            ContentTypeEnum.H5P => new H5PTestElement(name, shortname, parentSpace, content, url, authors,
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
        ParentSpace.SpaceLayout.RestoreMemento(_mementoSpaceLayout);
        
        _mappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}