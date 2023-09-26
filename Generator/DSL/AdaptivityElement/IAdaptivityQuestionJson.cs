namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityQuestionJson : IHasType
{
    string QuestionType { get; set; }
    int QuestionId { get; set; }
    string QuestionUUID { get; set; }
    int QuestionDifficulty { get; set; }
    string QuestionText { get; set; }
    List<IAdaptivityRuleJson> AdaptivityRules { get; set; }
    List<IChoiceJson> Choices { get; set; }
}