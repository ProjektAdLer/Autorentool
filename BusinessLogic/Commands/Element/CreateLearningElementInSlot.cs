using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands.Element;

public class CreateLearningElementInSlot : IUndoCommand
{
    public string Name => nameof(CreateLearningElementInSlot);
    internal LearningSpace ParentSpace { get; }
    internal int SlotIndex { get; }
    internal LearningElement LearningElement { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public CreateLearningElementInSlot(LearningSpace parentSpace, int slotIndex, string name, string shortName,
        LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY,
        Action<LearningSpace> mappingAction)
    {
        LearningElement = new LearningElement(name, shortName, learningContent, authors, description, goals,
            difficulty, parentSpace, workload, points, positionX, positionY);
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _mappingAction = mappingAction;
    }
    
    public CreateLearningElementInSlot(LearningSpace parentSpace, int slotIndex, LearningElement learningElement,
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