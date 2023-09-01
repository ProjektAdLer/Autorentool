namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

public class CommentAction : IAdaptivityAction
{
    public CommentAction(string comment)
    {
        Comment = comment;
    }
    
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentAction()
    {
        Comment = "";
    }

    public string Comment { get; set; }
}