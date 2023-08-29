namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

public class CommentAction : IAdaptivityAction
{
    public CommentAction(string comment)
    {
        Comment = comment;
    }

    public string Comment { get; set; }
}