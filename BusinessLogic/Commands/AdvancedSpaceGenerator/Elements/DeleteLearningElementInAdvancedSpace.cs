using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.AdvancedSpaceGenerator.Elements;

public class DeleteLearningElementInAdvancedSpace : IDeleteLearningElementInAdvancedSpace
{
    private IMemento? _memento;
    private IMemento? _mementoAdvancedSpaceLayout;

    public DeleteLearningElementInAdvancedSpace(LearningElement learningElement, AdvancedLearningSpace parentSpace,
        Action<AdvancedLearningSpace> mappingAction, ILogger<DeleteLearningElementInAdvancedSpace> logger)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningElement LearningElement { get; }
    internal AdvancedLearningSpace ParentSpace { get; }
    internal Action<AdvancedLearningSpace> MappingAction { get; }
    private ILogger<DeleteLearningElementInAdvancedSpace> Logger { get; }
    public string Name => nameof(DeleteLearningElementInAdvancedSpace);

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoAdvancedSpaceLayout = ParentSpace.AdvancedLearningSpaceLayout.GetMemento();

        ParentSpace.UnsavedChanges = true;
        var kvP = ParentSpace.AdvancedLearningSpaceLayout.LearningElements.First(x => x.Value.Id == LearningElement.Id);

        ParentSpace.AdvancedLearningSpaceLayout.LearningElements.Remove(kvP.Key);

        Logger.LogTrace(
            "Deleted LearningElement {LearningElementName} ({LearningElementId}) in AdvancedLearningSpace {AdvancedLearningSpaceName} ({AdvancedLearningSpaceId})",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        if (_mementoAdvancedSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoAdvancedSpaceLayout is null");
        }

        ParentSpace.RestoreMemento(_memento);
        ParentSpace.AdvancedLearningSpaceLayout.RestoreMemento(_mementoAdvancedSpaceLayout);

        Logger.LogTrace(
            "Undone deletion of LearningElement {LearningElementName} ({LearningElementId}) in AdvancedLearningSpace {AdvancedLearningSpaceName} ({AdvancedLearningSpaceId}). Restored AdvancedLearningSpace to previous state",
            LearningElement.Name, LearningElement.Id, ParentSpace.Name, ParentSpace.Id);

        MappingAction.Invoke(ParentSpace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningElementInAdvancedSpace");
        Execute();
    }
}