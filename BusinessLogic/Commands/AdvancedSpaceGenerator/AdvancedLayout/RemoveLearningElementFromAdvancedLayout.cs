using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.AdvancedSpaceGenerator.AdvancedLayout;

public class RemoveLearningElementFromAdvancedLayout : IRemoveLearningElementFromAdvancedLayout
{
    private IMemento? _mementoSpace;
    private IMemento? _mementoSpaceLayout;
    private IMemento? _mementoWorld;

    public RemoveLearningElementFromAdvancedLayout(LearningWorld learningWorld, AdvancedLearningSpace advancedLearningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction,
        ILogger<RemoveLearningElementFromAdvancedLayout> logger)
    {
        LearningWorld = learningWorld;
        AdvancedLearningSpace = (AdvancedLearningSpace)LearningWorld.LearningSpaces.First(x => x.Id == advancedLearningSpace.Id);
        LearningElement = AdvancedLearningSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal IAdvancedLearningSpace AdvancedLearningSpace { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<RemoveLearningElementFromAdvancedLayout> Logger { get; }
    public string Name => nameof(RemoveLearningElementFromAdvancedLayout);

    public void Execute()
    {
        _mementoWorld = LearningWorld.GetMemento();
        _mementoSpace = AdvancedLearningSpace.GetMemento();
        _mementoSpaceLayout = AdvancedLearningSpace.AdvancedLearningSpaceLayout.GetMemento();

        AdvancedLearningSpace.UnsavedChanges = true;

        var oldSlot =
            AdvancedLearningSpace.AdvancedLearningSpaceLayout.LearningElements.First(kvP => kvP.Value.Equals(LearningElement)).Key;

        AdvancedLearningSpace.AdvancedLearningSpaceLayout.LearningElements.Remove(oldSlot);

        if (LearningWorld.UnplacedLearningElements.Contains(LearningElement) == false)
        {
            LearningWorld.UnplacedLearningElements.Add(LearningElement);
        }

        Logger.LogTrace(
            "Removed LearningElement {LearningElementName} ({LearningElementId}) from slot {OldSlot} of AdvancedLearningSpace {AdvancedLearningSpaceName}({AdvancedLearningSpaceId}) in LearningWorld {LearningWorldName} ({LearningWorldId}) and added it to UnplacedLearningElements",
            LearningElement.Name, LearningElement.Id, oldSlot, AdvancedLearningSpace.Name, AdvancedLearningSpace.Id, LearningWorld.Name,
            LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_mementoWorld is null)
        {
            throw new InvalidOperationException("_mementoWorld is null");
        }

        if (_mementoSpace is null)
        {
            throw new InvalidOperationException("_mementoSpace is null");
        }

        if (_mementoSpaceLayout is null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        LearningWorld.RestoreMemento(_mementoWorld);
        AdvancedLearningSpace.RestoreMemento(_mementoSpace);
        AdvancedLearningSpace.AdvancedLearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        Logger.LogTrace(
            "Undone removal of LearningElement {LearningElementName} ({LearningElementId}). Restored LearningWorld {LearningWorldName} ({LearningWorldId}), LearningSpace {LearningSpaceName} ({LearningSpaceId}) and LearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, LearningWorld.Name, LearningWorld.Id, AdvancedLearningSpace.Name,
            AdvancedLearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing RemoveLearningElementFromLayout");
        Execute();
    }
}