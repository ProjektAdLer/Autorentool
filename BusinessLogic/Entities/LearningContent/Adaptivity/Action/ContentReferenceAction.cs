namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public class ContentReferenceAction : IAdaptivityAction
{
    /// <summary>
    /// Creates a new instance of <see cref="ContentReferenceAction"/>.
    /// </summary>
    /// <param name="content">Content to be referenced. Must not be <see cref="IAdaptivityContent"/>.</param>
    /// <param name="comment">Additional optional comment, may be empty string.</param>
    /// <exception cref="ArgumentException">Content was <see cref="IAdaptivityContent"/>.</exception>
    public ContentReferenceAction(ILearningContent content, string comment)
    {
        if (content is IAdaptivityContent)
            throw new ArgumentException("Content cannot be an adaptivity content", nameof(content));
        Content = content;
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ContentReferenceAction()
    {
        Content = null!;
        Id = Guid.Empty;
        Comment = "";
    }

    public ILearningContent Content { get; set; }
    public string Comment { get; set; }
    public Guid Id { get; private set; }

    public bool Equals(IAdaptivityAction? other)
    {
        if (other is not ContentReferenceAction contentReferenceAction)
            return false;
        return Content.Equals(contentReferenceAction.Content) &&
               Id.Equals(contentReferenceAction.Id) &&
               Comment.Equals(contentReferenceAction.Comment);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ContentReferenceAction)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Content, Id, Comment);
    }

    public static bool operator ==(ContentReferenceAction? left, ContentReferenceAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ContentReferenceAction? left, ContentReferenceAction? right)
    {
        return !Equals(left, right);
    }

    public IMemento GetMemento() => new ContentReferenceActionMemento(Id, Content);

    public void RestoreMemento(IMemento memento)
    {
        if(memento is not ContentReferenceActionMemento cram)
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        Id = cram.Id;
        Content = cram.Content;
    }

    private record ContentReferenceActionMemento : IMemento
    {
        public ContentReferenceActionMemento(Guid id, ILearningContent content)
        {
            Id = id;
            Content = content;
        }
        internal Guid Id { get; }
        internal ILearningContent Content { get; }
    }
}