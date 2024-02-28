using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Element;

public class DeleteStoryElementInSpace : IDeleteStoryElementInSpace
{
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public DeleteStoryElementInSpace(LearningElement learningElement, LearningSpace parentSpace,
        Action<LearningSpace> mappingAction, ILogger<DeleteStoryElementInSpace> logger)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningElement LearningElement { get; }
    internal LearningSpace ParentSpace { get; }
    internal Action<LearningSpace> MappingAction { get; }
    internal ILogger<DeleteStoryElementInSpace> Logger { get; }
    public string Name => nameof(DeleteStoryElementInSpace);

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.LearningSpaceLayout.GetMemento();

        ParentSpace.UnsavedChanges = true;
        var kvP = ParentSpace.LearningSpaceLayout.StoryElements.First(x => x.Value.Id == LearningElement.Id);

        ParentSpace.LearningSpaceLayout.StoryElements.Remove(kvP.Key);

        Logger.LogTrace(
            "Deleted StoryElement {LearningElementName} ({LearningElementId}) in LearningSpace {LearningSpaceName} ({LearningSpaceId})",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }

        ParentSpace.RestoreMemento(_memento);
        ParentSpace.LearningSpaceLayout.RestoreMemento(_mementoSpaceLayout);

        Logger.LogTrace(
            "Undone deletion of StoryElement {LearningElementName} ({LearningElementId}) in LearningSpace {LearningSpaceName} ({LearningSpaceId}). Restored LearningSpace to previous state",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteStoryElementInSpace");
        Execute();
    }
}