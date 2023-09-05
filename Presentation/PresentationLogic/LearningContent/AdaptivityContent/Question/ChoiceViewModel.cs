namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

public class ChoiceViewModel
{
    public ChoiceViewModel(string text)
    {
        Text = text;
    }

    private ChoiceViewModel()
    {
        Text = "";
    }

    public string Text { get; set; }
}