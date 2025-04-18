namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public class CommentAction : IAdaptivityAction
{
    public CommentAction(string comment)
    {
        Comment = comment;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    // ReSharper disable once UnusedMember.Local
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentAction()
    {
        Comment = "";
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    public string Comment { get; set; }
    public Guid Id { get; private set; }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }
    
    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

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

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Comment, Id);
    }
    // ReSharper restore NonReadonlyMemberInGetHashCode

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