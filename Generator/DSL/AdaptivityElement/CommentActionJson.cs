using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public class CommentActionJson : IAdaptivityActionJson
{
    [JsonConstructor]
    public CommentActionJson(string commentText)
    {
        CommentText = commentText;
    }

    public string CommentText { get; set; }
}