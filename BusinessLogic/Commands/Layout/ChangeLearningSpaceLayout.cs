using BusinessLogic.Entities;
using BusinessLogic.Entities.FloorPlans;
using Shared;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Layout;

public class ChangeLearningSpaceLayout : IChangeLearningSpaceLayout
{
    public string Name => nameof(ChangeLearningSpaceLayout);
    internal ILearningSpace LearningSpace { get; }
    public LearningWorld LearningWorld { get; }
    internal FloorPlanEnum FloorPlanName { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<LayoutCommandFactory> Logger { get; }
    private IMemento? _mementoSpaceLayout;
    private IMemento? _mementoSpace;
    private IMemento? _mementoWorld;

    public ChangeLearningSpaceLayout(ILearningSpace learningSpace, LearningWorld learningWorld,
        FloorPlanEnum floorPlanName, Action<LearningWorld> mappingAction, ILogger<LayoutCommandFactory> logger)
    {
        LearningSpace = learningSpace;
        LearningWorld = learningWorld;
        FloorPlanName = floorPlanName;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();
        _mementoSpace = LearningSpace.GetMemento();
        _mementoWorld = LearningWorld.GetMemento();

        LearningSpace.UnsavedChanges = true;
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
        foreach (var unplacedElement in remainingElements)
        {
            LearningWorld.UnsavedChanges = true;
            LearningWorld.UnplacedLearningElements.Add(unplacedElement.Value);
            unplacedElement.Value.Parent = null;
        }
        //compress the element indices if necessary
        if (newLearningElementDictionary.Any() && newLearningElementDictionary.Max(kvP => kvP.Key) >= capacity)
        {
            newLearningElementDictionary =
                newLearningElementDictionary
                    .Select((kvP, i) => new KeyValuePair<int, ILearningElement>(i, kvP.Value));
        }

        LearningSpace.LearningSpaceLayout.LearningElements =
            newLearningElementDictionary.ToDictionary(kvP => kvP.Key, kvP => kvP.Value);
        LearningSpace.LearningSpaceLayout.FloorPlanName = FloorPlanName;
        
        Logger.LogTrace("Changed LearningSpaceLayout for {LearningSpaceName} ({LearningSpaceId}) to {FloorPlan} in {LearningWorldName} ({LearningWorldId})", LearningSpace.Name, LearningSpace.Id, FloorPlanName.ToString(), LearningWorld.Name ,LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        if (_mementoSpace == null)
        {
            throw new InvalidOperationException("_mementoSpace is null");
        }

        if (_mementoWorld == null)
        {
            throw new InvalidOperationException("_mementoWorld is null");
        }
        
        LearningSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);
        LearningSpace.RestoreMemento(_mementoSpace);
        LearningWorld.RestoreMemento(_mementoWorld);

        Logger.LogTrace("Undone ChangeLearningSpaceLayout for {LearningSpaceName} ({LearningSpaceId}) in {LearningWorldName} ({LearningWorldId})", LearningSpace.Name, LearningSpace.Id, LearningWorld.Name ,LearningWorld.Id);
        
        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing ChangeLearningSpaceLayout");
        Execute();
    }
}