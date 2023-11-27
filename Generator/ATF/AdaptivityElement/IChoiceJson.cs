using System.Text.Json.Serialization;

namespace Generator.ATF.AdaptivityElement;

[JsonDerivedType(typeof(ChoiceJson), typeDiscriminator: JsonTypes.AdaptivityQuestionAnswerType)]
public interface IChoiceJson
{
    string AnswerText { get; set; }
    bool IsCorrect { get; set; }
}