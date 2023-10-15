using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.AdvancedSpace.AdvancedLayout;

public class PlaceLearningElementInAdvancedLayoutFromAdvancedLayout : IPlaceLearningElementInAdvancedLayoutFromAdvancedLayout
{
    private IMemento? _mementoLayout;
    private IMemento? _mementoSpace;

    public PlaceLearningElementInAdvancedLayoutFromAdvancedLayout(AdvancedLearningSpace parentSpace, ILearningElement learningElement,
        int newSlotIndex,
        Action<AdvancedLearningSpace> mappingAction, ILogger<PlaceLearningElementInAdvancedLayoutFromAdvancedLayout> logger)
    {
        ParentSpace = parentSpace;
        LearningElement = ParentSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        NewSlotIndex = newSlotIndex;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AdvancedLearningSpace ParentSpace { get; }
    internal int NewSlotIndex { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<AdvancedLearningSpace> MappingAction { get; }
    private ILogger<PlaceLearningElementInAdvancedLayoutFromAdvancedLayout> Logger { get; }
    public string Name => nameof(PlaceLearningElementInAdvancedLayoutFromAdvancedLayout);

    public void Execute()
    {
        _mementoLayout = ParentSpace.AdvancedLearningSpaceLayout.GetMemento();
        _mementoSpace = ParentSpace.GetMemento();

        ParentSpace.UnsavedChanges = true;

        var kvP = ParentSpace.AdvancedLearningSpaceLayout.LearningElements
            .First(kvP => kvP.Value.Equals(LearningElement));
        var oldSlotIndex = kvP.Key;
        ParentSpace.AdvancedLearningSpaceLayout.LearningElements.TryGetValue(NewSlotIndex, out var replacedLearningElement);
        ParentSpace.AdvancedLearningSpaceLayout.LearningElements[NewSlotIndex] = LearningElement;
        if (replacedLearningElement != null)
            ParentSpace.AdvancedLearningSpaceLayout.LearningElements[oldSlotIndex] = replacedLearningElement;
        else
            ParentSpace.AdvancedLearningSpaceLayout.LearningElements.Remove(oldSlotIndex);

        Logger.LogTrace(
            "Placed LearningElement {LearningElementName} ({LearningElementId}) from slot {OldSlotIndex} to {NewSlotIndex} of ParentSpace {ParentSpaceName}({ParentSpaceId})",
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
            "Undone placement of LearningElement {LearningElementName} ({LearningElementId}). Restored ParentSpace {ParentSpaceName} ({ParentSpaceId}) and AdvancedLearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing PlaceLearningElementInLayoutFromLayout");
        Execute();
    }
}