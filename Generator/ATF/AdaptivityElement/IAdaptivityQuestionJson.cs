using System.Text.Json.Serialization;

namespace Generator.ATF.AdaptivityElement;

[JsonDerivedType(typeof(AdaptivityQuestionJson), typeDiscriminator: JsonTypes.AdaptivityQuestionType)]
public interface IAdaptivityQuestionJson
{
    ResponseType QuestionType { get; set; }
    int QuestionId { get; set; }
    string QuestionUUID { get; set; }
    int QuestionDifficulty { get; set; }
    string QuestionText { get; set; }
    List<IAdaptivityRuleJson> AdaptivityRules { get; set; }
    List<IChoiceJson> Choices { get; set; }
}