using System.Text.Json.Serialization;

namespace Generator.ATF.AdaptivityElement;

public class ChoiceJson : IChoiceJson
{
    [JsonConstructor]
    public ChoiceJson(string answerText, bool isCorrect)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }

    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
}