namespace Generator.DSL.AdaptivityElement;

public class AdaptivityQuestionJson : IAdaptivityQuestionJson
{
    public AdaptivityQuestionJson(string type, string questionType, string questionUuid, int questionDifficulty,
        string questionText, List<IAdaptivityRuleJson> adaptivityRules)
    {
        Type = type;
        QuestionType = questionType;
        QuestionUUID = questionUuid;
        QuestionDifficulty = questionDifficulty;
        QuestionText = questionText;
        AdaptivityRules = adaptivityRules;
    }

    public string Type { get; }
    public string QuestionType { get; set; }
    public string QuestionUUID { get; set; }
    public int QuestionDifficulty { get; set; }
    public string QuestionText { get; set; }
    public List<IAdaptivityRuleJson> AdaptivityRules { get; set; }
}