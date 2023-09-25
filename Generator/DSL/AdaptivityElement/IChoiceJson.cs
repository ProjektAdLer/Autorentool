namespace Generator.DSL.AdaptivityElement;

public interface IChoiceJson : IHasType
{
    string AnswerText { get; set; }
    bool IsCorrect { get; set; }
}