using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityTaskJson
{
    [JsonPropertyName("$type")] string Type { get; }

    int TaskId { get; set; }
    string TaskUUID { get; set; }
    string TaskTitle { get; set; }
    bool Optional { get; set; }
    int RequiredDifficulty { get; set; }
    List<IAdaptivityQuestionJson> AdaptivityQuestions { get; set; }
}