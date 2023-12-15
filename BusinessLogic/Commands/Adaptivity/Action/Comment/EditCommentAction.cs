using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Action.Comment;

class EditCommentAction : IEditCommentAction
{
    public EditCommentAction(CommentAction action, string comment, Action<CommentAction> mappingAction,
        ILogger<EditCommentAction> logger)
    {
        Action = action;
        Comment = comment;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public string Name => nameof(EditCommentAction);
    internal CommentAction Action { get; }
    internal string Comment { get; }
    internal Action<CommentAction> MappingAction { get; }
    private ILogger<EditCommentAction> Logger { get; }
    private IMemento? Memento { get; set; }

    public void Execute()
    {
        Memento = Action.GetMemento();

        Action.Comment = Comment;
        Action.UnsavedChanges = true;
        MappingAction.Invoke(Action);

        Logger.LogTrace("Edited CommentAction {CommentActionId} with new comment {Comment}", Action.Id, Comment);
    }

    public void Undo()
    {
        if (Memento == null) throw new InvalidOperationException("Memento is null");

        Action.RestoreMemento(Memento);
        MappingAction.Invoke(Action);

        Logger.LogTrace("Undone editing of CommentAction {CommentActionId}", Action.Id);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditCommentAction");
        Execute();
    }
}