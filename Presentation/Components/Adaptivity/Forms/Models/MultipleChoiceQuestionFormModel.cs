using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

namespace Presentation.Components.Adaptivity.Forms.Models;

public class MultipleChoiceQuestionFormModel
{
    public MultipleChoiceQuestionFormModel()
    {
        Text = "";
        Choices = new List<ChoiceViewModel>();
        CorrectChoices = new List<ChoiceViewModel>();
        ExpectedCompletionTime = 5;
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsSingleResponse { get; set; }
    public ICollection<ChoiceViewModel> Choices { get; set; }
    public ICollection<ChoiceViewModel> CorrectChoices { get; set; }
    public int ExpectedCompletionTime { get; set; }
}