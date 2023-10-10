namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class ContentReferenceActionViewModel : IAdaptivityActionViewModel
{
    /// <summary>
    /// Creates a new instance of <see cref="ContentReferenceActionViewModel"/>.
    /// </summary>
    /// <param name="content">Content to be referenced. Must not be <see cref="IAdaptivityContentViewModel"/>.</param>
    /// <exception cref="ArgumentException">Content was <see cref="IAdaptivityContentViewModel"/>.</exception>
    public ContentReferenceActionViewModel(ILearningContentViewModel content)
    {
        if (content is IAdaptivityContentViewModel)
            throw new ArgumentException("Content cannot be an adaptivity content", nameof(content));
        Content = content;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ContentReferenceActionViewModel()
    {
        Content = null!;
        Id = Guid.Empty;
    }

    public ILearningContentViewModel Content { get; set; }
    public Guid Id { get; private set; }
}