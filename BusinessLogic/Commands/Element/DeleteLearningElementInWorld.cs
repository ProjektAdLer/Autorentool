using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Element;

public class DeleteLearningElementInWorld : IDeleteLearningElementInWorld
{
    private IMemento? _memento;

    public DeleteLearningElementInWorld(LearningElement learningElement, LearningWorld parentWorld,
        Action<LearningWorld> mappingAction, ILogger<DeleteLearningElementInWorld> logger)
    {
        LearningElement = learningElement;
        ParentWorld = parentWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningElement LearningElement { get; }
    internal LearningWorld ParentWorld { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<DeleteLearningElementInWorld> Logger { get; }
    public string Name => nameof(DeleteLearningElementInWorld);

    public void Execute()
    {
        _memento = ParentWorld.GetMemento();

        ParentWorld.UnsavedChanges = true;
        var element = ParentWorld.UnplacedLearningElements.First(x => x.Id == LearningElement.Id);
        ParentWorld.UnplacedLearningElements.Remove(element);

        Logger.LogTrace(
            "Deleted LearningElement {LearningElementName} ({LearningElementId}) in LearningWorld {LearningWorldName} ({LearningWorldId})",
            LearningElement.Name, LearningElement.Id, ParentWorld.Name, ParentWorld.Id);

        MappingAction.Invoke(ParentWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        ParentWorld.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone deletion of LearningElement {LearningElementName} ({LearningElementId}) in LearningWorld {LearningWorldName} ({LearningWorldId}). Restored LearningWorld to previous state",
            LearningElement.Name, LearningElement.Id, ParentWorld.Name, ParentWorld.Id);

        MappingAction.Invoke(ParentWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningElementInWorld");
        Execute();
    }
}