namespace Generator.DSL.AdaptivityElement;

public class ChoiceJson : IChoiceJson
{
    public ChoiceJson(string answerText, bool isCorrect)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }

    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public string Type => "AdaptivityQuestionChoice";
}