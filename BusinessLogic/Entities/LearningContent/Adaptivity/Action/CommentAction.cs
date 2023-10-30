namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public class CommentAction : IAdaptivityAction
{
    public CommentAction(string comment)
    {
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentAction()
    {
        Comment = "";
        Id = Guid.Empty;
    }

    public string Comment { get; set; }
    public Guid Id { get; private set; }

    public bool Equals(IAdaptivityAction? other)
    {
        if (other is not CommentAction commentAction)
            return false;
        return Comment == commentAction.Comment && Id.Equals(commentAction.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CommentAction)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Comment, Id);
    }

    public static bool operator ==(CommentAction? left, CommentAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CommentAction? left, CommentAction? right)
    {
        return !Equals(left, right);
    }

    public IMemento GetMemento() => new CommentActionMemento(Id, Comment);

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not CommentActionMemento cam)
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        Id = cam.Id;
        Comment = cam.Comment;
    }

    private record CommentActionMemento : IMemento
    {
        public CommentActionMemento(Guid id, string comment)
        {
            Id = id;
            Comment = comment;
        }

        public Guid Id { get; }
        public string Comment { get; }
    }
}