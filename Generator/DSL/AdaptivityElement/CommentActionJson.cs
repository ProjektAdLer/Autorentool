namespace Generator.DSL.AdaptivityElement;

public class CommentActionJson : IAdaptivityActionJson
{
    public CommentActionJson(string commentText)
    {
        CommentText = commentText;
    }

    public string CommentText { get; set; }

    public string Type => JsonTypes.CommentActionType;
}