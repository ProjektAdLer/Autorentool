using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places a learning element that is already contained in the layout into a different slot in that layout.
/// </summary>
public class PlaceLearningElementInLayoutFromLayout : IPlaceLearningElementInLayoutFromLayout
{
    private IMemento? _mementoLayout;
    private IMemento? _mementoSpace;

    public PlaceLearningElementInLayoutFromLayout(LearningSpace parentSpace, ILearningElement learningElement,
        int newSlotIndex,
        Action<LearningSpace> mappingAction, ILogger<PlaceLearningElementInLayoutFromLayout> logger)
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
    private ILogger<PlaceLearningElementInLayoutFromLayout> Logger { get; }
    public string Name => nameof(PlaceLearningElementInLayoutFromLayout);

    public void Execute()
    {
        _mementoLayout = ParentSpace.LearningSpaceLayout.GetMemento();
        _mementoSpace = ParentSpace.GetMemento();

        ParentSpace.UnsavedChanges = true;

        var kvP = ParentSpace.LearningSpaceLayout.LearningElements
            .First(kvP => kvP.Value.Equals(LearningElement));
        var oldSlotIndex = kvP.Key;
        var replacedLearningElement = ParentSpace.LearningSpaceLayout.LearningElements.ContainsKey(NewSlotIndex)
            ? ParentSpace.LearningSpaceLayout.LearningElements[NewSlotIndex]
            : null;
        ParentSpace.LearningSpaceLayout.LearningElements[NewSlotIndex] = LearningElement;
        if (replacedLearningElement != null)
            ParentSpace.LearningSpaceLayout.LearningElements[oldSlotIndex] = replacedLearningElement;
        else
            ParentSpace.LearningSpaceLayout.LearningElements.Remove(oldSlotIndex);

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
            "Undone placement of LearningElement {LearningElementName} ({LearningElementId}). Restored ParentSpace {ParentSpaceName} ({ParentSpaceId}) and LearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing PlaceLearningElementInLayoutFromLayout");
        Execute();
    }
}