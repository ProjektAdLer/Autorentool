namespace PersistEntities.LearningContent.Action;

public class ContentReferenceActionPe : IAdaptivityActionPe
{
    /// <summary>
    /// Creates a new instance of <see cref="ContentReferenceActionPe"/>.
    /// </summary>
    /// <param name="content">Content to be referenced. Must not be <see cref="IAdaptivityContentPe"/>.</param>
    /// <exception cref="ArgumentException">Content was <see cref="IAdaptivityContentPe"/>.</exception>
    public ContentReferenceActionPe(ILearningContentPe content)
    {
        if (content is IAdaptivityContentPe)
            throw new ArgumentException("Content cannot be an adaptivity content", nameof(content));
        Content = content;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ContentReferenceActionPe()
    {
        Content = null!;
        Id = Guid.Empty;
    }

    public ILearningContentPe Content { get; set; }
    public Guid Id { get; private set; }
}