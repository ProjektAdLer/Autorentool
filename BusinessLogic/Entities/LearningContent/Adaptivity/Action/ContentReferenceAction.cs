namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

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

    public bool Equals(IAdaptivityAction? other)
    {
        if (other is not ContentReferenceAction contentReferenceAction)
            return false;
        return Content.Equals(contentReferenceAction.Content) && Id.Equals(contentReferenceAction.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ContentReferenceAction) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Content, Id);
    }

    public static bool operator ==(ContentReferenceAction? left, ContentReferenceAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ContentReferenceAction? left, ContentReferenceAction? right)
    {
        return !Equals(left, right);
    }
}