namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

public class CommentAction : IAdaptivityAction
{
    public CommentAction(string comment)
    {
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentAction()
    {
        Comment = "";
        Id = Guid.Empty;
    }

    public string Comment { get; set; }
    public Guid Id { get; private set; }
}