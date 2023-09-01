namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class CommentActionViewModel : IAdaptivityActionViewModel
{
    public CommentActionViewModel(string comment)
    {
        Comment = comment;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentActionViewModel()
    {
        Comment = "";
    }

    public string Comment { get; set; }
}