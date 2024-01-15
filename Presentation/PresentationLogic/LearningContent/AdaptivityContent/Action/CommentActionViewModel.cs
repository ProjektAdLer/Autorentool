namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class CommentActionViewModel : IAdaptivityActionViewModel
{
    public CommentActionViewModel(string comment)
    {
        Comment = comment;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentActionViewModel()
    {
        Comment = "";
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    public string Comment { get; set; }
    public Guid Id { get; private set; }
    public bool UnsavedChanges { get; set; }
}