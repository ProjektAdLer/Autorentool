using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class LoadLearningElement : ILoadLearningElement
{
    public string Name => nameof(LoadLearningElement);
    internal IBusinessLogic BusinessLogic { get; }
    
    internal LearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement? LearningElement { get; private set; }
    internal string Filepath { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public LoadLearningElement(LearningSpace parentSpace, int slotIndex, string filepath, IBusinessLogic businessLogic, 
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        Filepath = filepath;
        BusinessLogic = businessLogic;
        MappingAction = mappingAction;
    }
    
    public LoadLearningElement(LearningSpace parentSpace, int slotIndex, Stream stream, IBusinessLogic businessLogic,
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        Filepath = "";
        BusinessLogic = businessLogic;
        LearningElement = BusinessLogic.LoadLearningElement(stream);
        MappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();
        
        LearningElement ??= BusinessLogic.LoadLearningElement(Filepath);
        LearningElement.Parent = ParentSpace;
        ParentSpace.LearningSpaceLayout.LearningElements[SlotIndex] = LearningElement;

        MappingAction.Invoke(ParentSpace);
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
        
        MappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}