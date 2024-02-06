using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Layout;

public class PlaceStoryElementInLayoutFromLayout : IPlaceStoryElementInLayoutFromLayout
{
    private IMemento? _mementoLayout;
    private IMemento? _mementoSpace;

    public PlaceStoryElementInLayoutFromLayout(LearningSpace parentSpace, ILearningElement learningElement,
        int newSlotIndex, Action<LearningSpace> mappingAction,
        ILogger<PlaceStoryElementInLayoutFromLayout> logger)
    {
        ParentSpace = parentSpace;
        LearningElement = ParentSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newSlotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }
    internal LearningSpace ParentSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<LearningSpace> MappingAction { get; }
    private ILogger<PlaceStoryElementInLayoutFromLayout> Logger { get; }
    public string Name => nameof(PlaceStoryElementInLayoutFromLayout);
    public void Execute()
    {
        _mementoLayout = ParentSpace.LearningSpaceLayout.GetMemento();
        _mementoSpace = ParentSpace.GetMemento();

        ParentSpace.UnsavedChanges = true;

        var kvP = ParentSpace.LearningSpaceLayout.StoryElements
            .First(kvP => kvP.Value.Equals(LearningElement));
        var oldSlotIndex = kvP.Key;
        ParentSpace.LearningSpaceLayout.StoryElements.TryGetValue(NewSlotIndex, out var replacedLearningElement);
        ParentSpace.LearningSpaceLayout.StoryElements[NewSlotIndex] = LearningElement;
        if (replacedLearningElement != null)
            ParentSpace.LearningSpaceLayout.StoryElements[oldSlotIndex] = replacedLearningElement;
        else
            ParentSpace.LearningSpaceLayout.StoryElements.Remove(oldSlotIndex);

        Logger.LogTrace(
            "Placed StoryElement {LearningElementName} ({LearningElementId}) from slot {OldSlotIndex} to {NewSlotIndex} of ParentSpace {ParentSpaceName}({ParentSpaceId})",
            LearningElement.Name, LearningElement.Id, oldSlotIndex, NewSlotIndex, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_mementoLayout == null)
        {
            throw new InvalidOperationException("_mementoLayout is null");
        }

        if (_mementoSpace == null)
        {
            throw new InvalidOperationException("_mementoSpace is null");
        }

        ParentSpace.LearningSpaceLayout.RestoreMemento(_mementoLayout);
        ParentSpace.RestoreMemento(_mementoSpace);

        Logger.LogTrace(
            "Undone placement of StoryElement {LearningElementName} ({LearningElementId}). Restored ParentSpace {ParentSpaceName} ({ParentSpaceId}) and LearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing PlaceStoryElementInLayoutFromLayout");
        Execute();
    }
}