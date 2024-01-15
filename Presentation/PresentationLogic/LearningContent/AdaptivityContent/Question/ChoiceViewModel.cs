namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

public class ChoiceViewModel
{
    public ChoiceViewModel(string text)
    {
        Text = text;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ChoiceViewModel()
    {
        Text = "";
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    public string Text { get; set; }
    public Guid Id { get; set; }
    public bool UnsavedChanges { get; set; }
}