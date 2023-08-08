using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places an unplaced learning element in a slot in a layout.
/// </summary>
public class PlaceLearningElementInLayoutFromUnplaced : IPlaceLearningElementInLayoutFromUnplaced
{
    private IMemento? _mementoSpace;
    private IMemento? _mementoSpaceLayout;
    private IMemento? _mementoWorld;

    public PlaceLearningElementInLayoutFromUnplaced(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningWorld> mappingAction,
        ILogger<PlaceLearningElementInLayoutFromUnplaced> logger)
    {
        LearningWorld = learningWorld;
        LearningSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningSpace.Id);
        LearningElement = LearningWorld.UnplacedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newSlotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal ILearningSpace LearningSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<PlaceLearningElementInLayoutFromUnplaced> Logger { get; }
    public string Name => nameof(PlaceLearningElementInLayoutFromUnplaced);

    public void Execute()
    {
        _mementoWorld = LearningWorld.GetMemento();
        _mementoSpace = LearningSpace.GetMemento();
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();

        LearningSpace.UnsavedChanges = true;
        LearningWorld.UnsavedChanges = true;

        if (LearningWorld.UnplacedLearningElements.Contains(LearningElement))
        {
            LearningWorld.UnplacedLearningElements.Remove(LearningElement);
        }

        LearningSpace.LearningSpaceLayout.LearningElements.TryGetValue(NewSlotIndex, out var oldElement);
        if (oldElement != null)
        {
            if (LearningWorld.UnplacedLearningElements.Contains(oldElement) == false)
            {
                LearningWorld.UnplacedLearningElements.Add(oldElement);
            }
        }

        LearningSpace.LearningSpaceLayout.LearningElements[NewSlotIndex] = LearningElement;

        Logger.LogTrace(
            "Placed LearningElement {LearningElementName} ({LearningElementId}) from UnplacedLearningElements to slot {NewSlotIndex} of LearningSpace {LearningSpaceName}({LearningSpaceId}) in LearningWorld {LearningWorldName} ({LearningWorldId})",
            LearningElement.Name, LearningElement.Id, NewSlotIndex, LearningSpace.Name, LearningSpace.Id,
            LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_mementoWorld == null)
        {
            throw new InvalidOperationException("_mementoWorld is null");
        }

        if (_mementoSpace == null)
        {
            throw new InvalidOperationException("_mementoSpace is null");
        }

        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        LearningWorld.RestoreMemento(_mementoWorld);
        LearningSpace.RestoreMemento(_mementoSpace);
        LearningSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        Logger.LogTrace(
            "Undone placement of LearningElement {LearningElementName} ({LearningElementId}) from UnplacedLearningElements. Restored LearningWorld {LearningWorldName} ({LearningWorldId}), LearningSpace {LearningSpaceName} ({LearningSpaceId}) and LearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, LearningWorld.Name, LearningWorld.Id, LearningSpace.Name,
            LearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing PlaceLearningElementInLayoutFromUnplaced");
        Execute();
    }
}