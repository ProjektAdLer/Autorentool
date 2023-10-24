using System.Text.Json.Serialization;

namespace Generator.ATF.AdaptivityElement;

public class AdaptivityQuestionJson : IAdaptivityQuestionJson
{
    [JsonConstructor]
    public AdaptivityQuestionJson(ResponseType questionType, int questionId, string questionUuid,
        int questionDifficulty,
        string questionText, List<IAdaptivityRuleJson> adaptivityRules, List<IChoiceJson> choices)
    {
        QuestionType = questionType;
        QuestionId = questionId;
        QuestionUUID = questionUuid;
        QuestionDifficulty = questionDifficulty;
        QuestionText = questionText;
        AdaptivityRules = adaptivityRules;
        Choices = choices;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ResponseType QuestionType { get; set; }

    public int QuestionId { get; set; }
    public string QuestionUUID { get; set; }
    public int QuestionDifficulty { get; set; }
    public string QuestionText { get; set; }
    public List<IAdaptivityRuleJson> AdaptivityRules { get; set; }
    public List<IChoiceJson> Choices { get; set; }
}