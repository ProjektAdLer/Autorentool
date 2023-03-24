using BusinessLogic.Entities;
using BusinessLogic.Entities.FloorPlans;
using Shared;

namespace BusinessLogic.Commands.Layout;

public class ChangeLearningSpaceLayout : IUndoCommand
{
    public string Name => nameof(ChangeLearningSpaceLayout);
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
        //TODO: save both space and containing world memento
        _memento = LearningSpace.LearningSpaceLayout.GetMemento();
        var capacity = FloorPlanProvider.GetFloorPlan(FloorPlanName).Capacity;
        IEnumerable<KeyValuePair<int, ILearningElement>> newLearningElementDictionary =
            LearningSpace.LearningSpaceLayout.LearningElements
                .OrderBy(kvP => kvP.Key)
                .Take(capacity)
                .ToList();
        //elements that won't fit on the new floor plan
        //TODO: put these in the world
        var remainingElements = LearningSpace.LearningSpaceLayout.LearningElements
            .OrderBy(kvP => kvP.Key)
            .Skip(capacity);
        //compress the element indices if necessary
        if (newLearningElementDictionary.Max(kvP => kvP.Key) >= capacity)
        {
            newLearningElementDictionary =
                newLearningElementDictionary
                    .Select((kvP, i) => new KeyValuePair<int, ILearningElement>(i, kvP.Value));
        }

        LearningSpace.LearningSpaceLayout.LearningElements =
            newLearningElementDictionary.ToDictionary(kvP => kvP.Key, kvP => kvP.Value);
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