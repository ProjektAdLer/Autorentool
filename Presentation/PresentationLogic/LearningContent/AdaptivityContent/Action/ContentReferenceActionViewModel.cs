namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class ContentReferenceActionViewModel : IAdaptivityActionViewModel
{
    /// <summary>
    /// Creates a new instance of <see cref="ContentReferenceActionViewModel"/>.
    /// </summary>
    /// <param name="content">Content to be referenced. Must not be <see cref="IAdaptivityContentViewModel"/>.</param>
    /// <param name="comment">Comment for the action.</param>
    /// <exception cref="ArgumentException">Content was <see cref="IAdaptivityContentViewModel"/>.</exception>
    public ContentReferenceActionViewModel(ILearningContentViewModel content, string comment)
    {
        if (content is IAdaptivityContentViewModel)
            throw new ArgumentException(@"Content cannot be an adaptivity content", nameof(content));
        Content = content;
        Comment = comment;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    // ReSharper disable once UnusedMember.Local
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ContentReferenceActionViewModel()
    {
        Content = null!;
        Comment = "";
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    public ILearningContentViewModel Content { get; set; }
    public string Comment { get; set; } 
    public Guid Id { get; private set; }
    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Content.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

}