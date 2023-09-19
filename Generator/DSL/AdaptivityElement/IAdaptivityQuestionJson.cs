using System.Text.Json.Serialization;

namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityQuestionJson
{
    [JsonPropertyName("$type")] string Type { get; }

    string QuestionType { get; set; }
    string QuestionUUID { get; set; }
    int QuestionDifficulty { get; set; }
    string QuestionText { get; set; }
    List<IAdaptivityRuleJson> AdaptivityRules { get; set; }
}