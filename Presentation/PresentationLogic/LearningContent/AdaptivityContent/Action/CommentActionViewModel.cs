namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class CommentActionViewModel : IAdaptivityActionViewModel
{
    public CommentActionViewModel(string comment)
    {
        Comment = comment;
    }

    public string Comment { get; set; }
}