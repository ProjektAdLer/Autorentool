using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands.Element;

public class CreateLearningElementInSlot : ICreateLearningElementInSlot
{
    public string Name => nameof(CreateLearningElementInSlot);
    internal LearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement LearningElement { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public CreateLearningElementInSlot(LearningSpace parentSpace, int slotIndex, string name, 
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY,
        Action<LearningSpace> mappingAction)
    {
        LearningElement = new LearningElement(name, learningContent, description, goals,
            difficulty, parentSpace, workload: workload, points: points, positionX: positionX, positionY: positionY);
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        MappingAction = mappingAction;
    }
    
    public CreateLearningElementInSlot(LearningSpace parentSpace, int slotIndex, LearningElement learningElement,
        Action<LearningSpace> mappingAction)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();

        ParentSpace.UnsavedChanges = true;
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