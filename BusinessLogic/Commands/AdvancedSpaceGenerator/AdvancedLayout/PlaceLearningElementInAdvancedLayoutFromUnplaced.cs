using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.AdvancedSpace.AdvancedLayout;

public class PlaceLearningElementInAdvancedLayoutFromUnplaced : IPlaceLearningElementInAdvancedLayoutFromUnplaced
{
    private IMemento? _mementoAdvancedSpace;
    private IMemento? _mementoAdvancedSpaceLayout;
    private IMemento? _mementoWorld;

    public PlaceLearningElementInAdvancedLayoutFromUnplaced(LearningWorld learningWorld, AdvancedLearningSpace advancedLearningSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningWorld> mappingAction,
        ILogger<PlaceLearningElementInAdvancedLayoutFromUnplaced> logger)
    {
        LearningWorld = learningWorld;
        AdvancedLearningSpace = (IAdvancedLearningSpace)LearningWorld.LearningSpaces.First(x => x.Id == advancedLearningSpace.Id);
        LearningElement = LearningWorld.UnplacedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newSlotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal IAdvancedLearningSpace AdvancedLearningSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<PlaceLearningElementInAdvancedLayoutFromUnplaced> Logger { get; }
    public string Name => nameof(PlaceLearningElementInAdvancedLayoutFromUnplaced);

    public void Execute()
    {
        _mementoWorld = LearningWorld.GetMemento();
        _mementoAdvancedSpace = AdvancedLearningSpace.GetMemento();
        _mementoAdvancedSpaceLayout = AdvancedLearningSpace.AdvancedLearningSpaceLayout.GetMemento();

        AdvancedLearningSpace.UnsavedChanges = true;
        LearningWorld.UnsavedChanges = true;

        if (LearningWorld.UnplacedLearningElements.Contains(LearningElement))
        {
            LearningWorld.UnplacedLearningElements.Remove(LearningElement);
        }

        AdvancedLearningSpace.AdvancedLearningSpaceLayout.LearningElements.TryGetValue(NewSlotIndex, out var oldElement);
        if (oldElement != null)
        {
            if (LearningWorld.UnplacedLearningElements.Contains(oldElement) == false)
            {
                LearningWorld.UnplacedLearningElements.Add(oldElement);
            }
        }

        AdvancedLearningSpace.AdvancedLearningSpaceLayout.LearningElements[NewSlotIndex] = LearningElement;

        Logger.LogTrace(
            "Placed LearningElement {LearningElementName} ({LearningElementId}) from UnplacedLearningElements to slot {NewSlotIndex} of AdvancedLearningSpace {AdvancedLearningSpaceName}({AdvancedLearningSpaceId}) in LearningWorld {LearningWorldName} ({LearningWorldId})",
            LearningElement.Name, LearningElement.Id, NewSlotIndex, AdvancedLearningSpace.Name, AdvancedLearningSpace.Id,
            LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_mementoWorld == null)
        {
            throw new InvalidOperationException("_mementoWorld is null");
        }

        if (_mementoAdvancedSpace == null)
        {
            throw new InvalidOperationException("_mementoAdvancedSpace is null");
        }

        if (_mementoAdvancedSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoAdvancedSpaceLayout is null");
        }

        LearningWorld.RestoreMemento(_mementoWorld);
        AdvancedLearningSpace.RestoreMemento(_mementoAdvancedSpace);
        AdvancedLearningSpace.AdvancedLearningSpaceLayout.RestoreMemento(_mementoAdvancedSpaceLayout);

        Logger.LogTrace(
            "Undone placement of LearningElement {LearningElementName} ({LearningElementId}) from UnplacedLearningElements. Restored LearningWorld {LearningWorldName} ({LearningWorldId}), AdvancedLearningSpace {AdvancedLearningSpaceName} ({AdvancedLearningSpaceId}) and LearningAdvancedSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, LearningWorld.Name, LearningWorld.Id, AdvancedLearningSpace.Name,
            AdvancedLearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing PlaceLearningElementInAdvancedLayoutFromUnplaced");
        Execute();
    }
}