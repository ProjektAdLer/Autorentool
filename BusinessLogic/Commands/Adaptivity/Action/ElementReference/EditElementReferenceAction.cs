using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Action.ElementReference;

class EditElementReferenceAction : IEditElementReferenceAction
{
    public EditElementReferenceAction(ElementReferenceAction action, Guid elementId,
        string comment,
        Action<ElementReferenceAction> mappingAction, ILogger<EditElementReferenceAction> logger)
    {
        Action = action;
        ElementId = elementId;
        Comment = comment;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public string Name => nameof(EditElementReferenceAction);
    internal ElementReferenceAction Action { get; }
    internal Guid ElementId { get; }
    public string Comment { get; }
    internal Action<ElementReferenceAction> MappingAction { get; }
    private ILogger<EditElementReferenceAction> Logger { get; }
    private IMemento? Memento { get; set; }

    public void Execute()
    {
        Memento = Action.GetMemento();

        Action.ElementId = ElementId;
        Action.Comment = Comment;
        MappingAction.Invoke(Action);

        Logger.LogTrace("Edited ElementReferenceAction {ElementReferenceActionId} in AdaptivityRule {AdaptivityRuleId}",
            Action.Id, Action.ElementId);
    }

    public void Undo()
    {
        if (Memento == null) throw new InvalidOperationException("Memento is null");

        Action.RestoreMemento(Memento);
        MappingAction.Invoke(Action);

        Logger.LogTrace(
            "Undone editing of ElementReferenceAction {ElementReferenceActionId} in AdaptivityRule {AdaptivityRuleId}",
            Action.Id, Action.ElementId);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditElementReferenceAction");
        Execute();
    }
}