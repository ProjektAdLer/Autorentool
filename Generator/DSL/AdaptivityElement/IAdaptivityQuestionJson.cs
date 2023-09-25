namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityQuestionJson : IHasType
{
    string QuestionType { get; set; }
    string QuestionId { get; set; }
    string QuestionUUID { get; set; }
    int QuestionDifficulty { get; set; }
    string QuestionText { get; set; }
    List<IAdaptivityRuleJson> AdaptivityRules { get; set; }
}