namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

public class MultipleChoiceSingleResponseQuestion : IMultipleChoiceQuestion
{
    public MultipleChoiceSingleResponseQuestion(int expectedCompletionTime, IEnumerable<Choice> choices,
        IEnumerable<Choice> correctChoices, string text)
    {
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        CorrectChoices = correctChoices;
        Text = text;
    }

    public int ExpectedCompletionTime { get; set; }
    public IEnumerable<Choice> Choices { get; set; }
    public IEnumerable<Choice> CorrectChoices { get; set; }
    public string Text { get; set; }
}