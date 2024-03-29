using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Layout;

public class RemoveLearningElementFromLayout : IRemoveLearningElementFromLayout
{
    private IMemento? _mementoSpace;
    private IMemento? _mementoSpaceLayout;
    private IMemento? _mementoWorld;

    public RemoveLearningElementFromLayout(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction,
        ILogger<RemoveLearningElementFromLayout> logger)
    {
        LearningWorld = learningWorld;
        LearningSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningSpace.Id);
        LearningElement = LearningSpace.ContainedLearningElements.First(x => x.Id == learningElement.Id);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal ILearningSpace LearningSpace { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<RemoveLearningElementFromLayout> Logger { get; }
    public string Name => nameof(RemoveLearningElementFromLayout);

    public void Execute()
    {
        _mementoWorld = LearningWorld.GetMemento();
        _mementoSpace = LearningSpace.GetMemento();
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();

        LearningSpace.UnsavedChanges = true;

        var oldSlot =
            LearningSpace.LearningSpaceLayout.LearningElements.First(kvP => kvP.Value.Equals(LearningElement)).Key;

        LearningSpace.LearningSpaceLayout.LearningElements.Remove(oldSlot);

        if (LearningWorld.UnplacedLearningElements.Contains(LearningElement) == false)
        {
            LearningWorld.UnplacedLearningElements.Add(LearningElement);
        }

        Logger.LogTrace(
            "Removed LearningElement {LearningElementName} ({LearningElementId}) from slot {OldSlot} of LearningSpace {LearningSpaceName}({LearningSpaceId}) in LearningWorld {LearningWorldName} ({LearningWorldId}) and added it to UnplacedLearningElements",
            LearningElement.Name, LearningElement.Id, oldSlot, LearningSpace.Name, LearningSpace.Id, LearningWorld.Name,
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
        LearningSpace.RestoreMemento(_mementoSpace);
        LearningSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        Logger.LogTrace(
            "Undone removal of LearningElement {LearningElementName} ({LearningElementId}). Restored LearningWorld {LearningWorldName} ({LearningWorldId}), LearningSpace {LearningSpaceName} ({LearningSpaceId}) and LearningSpaceLayout to previous state",
            LearningElement.Name, LearningElement.Id, LearningWorld.Name, LearningWorld.Id, LearningSpace.Name,
            LearningSpace.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing RemoveLearningElementFromLayout");
        Execute();
    }
}