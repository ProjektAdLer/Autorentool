namespace Presentation.Components.Adaptivity.Forms.Models;

public class MultipleChoiceQuestionFormModel
{
    public MultipleChoiceQuestionFormModel()
    {
        Text = "";
        Choices = new List<ChoiceFormModel>();
        CorrectChoices = new List<ChoiceFormModel>();
        ExpectedCompletionTime = 5;
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsSingleResponse { get; set; }
    public IList<ChoiceFormModel> Choices { get; set; }
    public ICollection<ChoiceFormModel> CorrectChoices { get; set; }
    public int ExpectedCompletionTime { get; set; }
}