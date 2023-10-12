using BusinessLogic.Commands.Adaptivity.Action.Comment;
using BusinessLogic.Commands.Adaptivity.Action.ContentReference;
using BusinessLogic.Commands.Adaptivity.Action.ElementReference;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;

namespace BusinessLogic.Commands.Adaptivity.Action;

public interface IAdaptivityActionCommandFactory
{
    IEditCommentAction GetEditCommentAction(CommentAction action, string comment, Action<CommentAction> mappingAction);

    IEditContentReferenceAction GetEditContentReferenceAction(ContentReferenceAction action, ILearningContent content,
        Action<ContentReferenceAction> mappingAction);
 
    IEditElementReferenceAction GetEditElementReferenceAction(ElementReferenceAction action, Guid elementId,
        Action<ElementReferenceAction> mappingAction);
}