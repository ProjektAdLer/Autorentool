using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Action.ContentReference;

class EditContentReferenceAction : IEditContentReferenceAction
{
    public EditContentReferenceAction(ContentReferenceAction action, ILearningContent content,
        Action<ContentReferenceAction> mappingAction, ILogger<EditContentReferenceAction> logger)
    {
        Action = action;
        Content = content;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public string Name => nameof(EditContentReferenceAction);
    internal ContentReferenceAction Action { get; }
    internal ILearningContent Content { get; }
    internal Action<ContentReferenceAction> MappingAction { get; }
    private ILogger<EditContentReferenceAction> Logger { get; }
    private IMemento? Memento { get; set; }


    public void Execute()
    {
        Memento = Action.GetMemento();

        Action.Content = Content;
        MappingAction.Invoke(Action);

        Logger.LogTrace("Edited ContentReferenceAction {ContentReferenceActionId}", Action.Id);
    }

    public void Undo()
    {
        if (Memento == null) throw new InvalidOperationException("Memento is null");

        Action.RestoreMemento(Memento);
        MappingAction.Invoke(Action);

        Logger.LogTrace("Undone editing of ContentReferenceAction {ContentReferenceActionId}", Action.Id);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditContentReferenceAction");
        Execute();
    }
}