using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class LoadLearningElement : ILoadLearningElement
{
    public string Name => nameof(LoadLearningElement);
    private readonly IBusinessLogic _businessLogic;
    
    internal LearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement? LearningElement;
    private readonly string _filepath;
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public LoadLearningElement(LearningSpace parentSpace, int slotIndex, string filepath, IBusinessLogic businessLogic, 
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    
    public LoadLearningElement(LearningSpace parentSpace, int slotIndex, Stream stream, IBusinessLogic businessLogic,
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _filepath = "";
        _businessLogic = businessLogic;
        LearningElement = _businessLogic.LoadLearningElement(stream);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();
        
        LearningElement ??= _businessLogic.LoadLearningElement(_filepath);
        LearningElement.Parent = ParentSpace;
        ParentSpace.LearningSpaceLayout.LearningElements[SlotIndex] = LearningElement;

        _mappingAction.Invoke(ParentSpace);
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