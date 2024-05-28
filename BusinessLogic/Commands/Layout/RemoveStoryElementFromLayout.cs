using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Layout;

public class RemoveStoryElementFromLayout : IRemoveStoryElementFromLayout
{
    private IMemento? _mementoSpaceLayout;
    internal IMemento? MementoSpace;
    internal IMemento? MementoWorld;

    public RemoveStoryElementFromLayout(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction,
        ILogger<RemoveStoryElementFromLayout> logger)
    {
        LearningWorld = learningWorld;
        LearningSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningSpace.Id);
        LearningElement = LearningSpace.LearningSpaceLayout.StoryElements.First(x => x.Value.Id == learningElement.Id)
            .Value;
        MappingAction = mappingAction;
        Logger = logger;
    }


    internal LearningWorld LearningWorld { get; }
    internal ILearningSpace LearningSpace { get; }
    internal ILearningElement LearningElement { get; }
    internal Action<LearningWorld> MappingAction { get; }
    internal ILogger<RemoveStoryElementFromLayout> Logger { get; }
    public string Name => nameof(RemoveStoryElementFromLayout);

    public void Execute()
    {
        MementoWorld = LearningWorld.GetMemento();
        MementoSpace = LearningSpace.GetMemento();
        _mementoSpaceLayout = LearningSpace.LearningSpaceLayout.GetMemento();

        LearningSpace.UnsavedChanges = true;

        var oldSlot =
            LearningSpace.LearningSpaceLayout.StoryElements.First(kvP => kvP.Value.Equals(LearningElement)).Key;

        LearningSpace.LearningSpaceLayout.StoryElements.Remove(oldSlot);

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
        if (MementoWorld is null)
        {
            throw new InvalidOperationException("MementoWorld is null");
        }

        if (MementoSpace is null)
        {
            throw new InvalidOperationException("MementoSpace is null");
        }

        if (_mementoSpaceLayout is null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        LearningWorld.RestoreMemento(MementoWorld);
        LearningSpace.RestoreMemento(MementoSpace);
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