namespace Generator.DSL.AdaptivityElement;

public class CommentActionJson : IAdaptivityActionJson
{
    public CommentActionJson(string type, string commentText)
    {
        Type = type;
        CommentText = commentText;
    }

    public string CommentText { get; set; }

    public string Type { get; }
}