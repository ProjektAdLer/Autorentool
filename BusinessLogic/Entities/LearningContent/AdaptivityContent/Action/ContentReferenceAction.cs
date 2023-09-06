namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

public class ContentReferenceAction : IAdaptivityAction
{
    /// <summary>
    /// Creates a new instance of <see cref="ContentReferenceAction"/>.
    /// </summary>
    /// <param name="content">Content to be referenced. Must not be <see cref="IAdaptivityContent"/>.</param>
    /// <exception cref="ArgumentException">Content was <see cref="IAdaptivityContent"/>.</exception>
    public ContentReferenceAction(ILearningContent content)
    {
        if (content is IAdaptivityContent)
            throw new ArgumentException("Content cannot be an adaptivity content", nameof(content));
        Content = content;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ContentReferenceAction()
    {
        Content = null!;
        Id = Guid.Empty;
    }

    public ILearningContent Content { get; set; }
    public Guid Id { get; private set; }
}