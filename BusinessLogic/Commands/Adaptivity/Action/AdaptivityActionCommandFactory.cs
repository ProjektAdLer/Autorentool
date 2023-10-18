using BusinessLogic.Commands.Adaptivity.Action.Comment;
using BusinessLogic.Commands.Adaptivity.Action.ContentReference;
using BusinessLogic.Commands.Adaptivity.Action.ElementReference;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Action;

public class AdaptivityActionCommandFactory : IAdaptivityActionCommandFactory
{
    public AdaptivityActionCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    public ILoggerFactory LoggerFactory { get; }

    public IEditCommentAction GetEditCommentAction(CommentAction action, string comment,
        Action<CommentAction> mappingAction)
    {
        return new EditCommentAction(action, comment, mappingAction, LoggerFactory.CreateLogger<EditCommentAction>());
    }

    public IEditContentReferenceAction GetEditContentReferenceAction(ContentReferenceAction action,
        ILearningContent content, string comment,
        Action<ContentReferenceAction> mappingAction)
    {
        return new EditContentReferenceAction(action, content, comment, mappingAction,
            LoggerFactory.CreateLogger<EditContentReferenceAction>());
    }

    public IEditElementReferenceAction GetEditElementReferenceAction(ElementReferenceAction action, Guid elementId,
        string comment,
        Action<ElementReferenceAction> mappingAction)
    {
        return new EditElementReferenceAction(action, elementId, comment, mappingAction,
            LoggerFactory.CreateLogger<EditElementReferenceAction>());
    }
}