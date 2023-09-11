namespace PersistEntities.LearningContent.Action;

public class CommentActionPe : IAdaptivityActionPe
{
    public CommentActionPe(string comment)
    {
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentActionPe()
    {
        Comment = "";
        Id = Guid.Empty;
    }

    public string Comment { get; set; }
    public Guid Id { get; private set; }
}