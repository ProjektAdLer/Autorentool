using BusinessLogic.Entities;
using BusinessLogic.Entities.FloorPlans;
using Shared;

namespace BusinessLogic.Commands;

public class ChangeLearningSpaceLayout : IUndoCommand
{
    internal LearningSpace LearningSpace { get; }
    internal FloorPlanEnum FloorPlanName { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public ChangeLearningSpaceLayout(LearningSpace learningSpace, FloorPlanEnum floorPlanName, Action<LearningSpace> mappingAction)
    {
        LearningSpace = learningSpace;
        FloorPlanName = floorPlanName;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningSpace.LearningSpaceLayout.GetMemento();

        var newLearningElementArray = new ILearningElement?[FloorPlanProvider.GetFloorPlan(FloorPlanName).Capacity];
        for (int i = 0; i < Math.Min(LearningSpace.LearningSpaceLayout.LearningElements.Length, newLearningElementArray.Length); i++)
        {
            newLearningElementArray[i] = LearningSpace.LearningSpaceLayout.LearningElements[i];
        }
        // var newLearningSpaceLayout = new LearningSpaceLayout(newLearningElementArray, FloorPlanName);
        LearningSpace.LearningSpaceLayout.LearningElements = newLearningElementArray;
        LearningSpace.LearningSpaceLayout.FloorPlanName = FloorPlanName;
        
        _mappingAction.Invoke(LearningSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningSpace.LearningSpaceLayout.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningSpace);
    }

    public void Redo() => Execute();
}