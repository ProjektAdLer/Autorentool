namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public class ElementReferenceAction : IAdaptivityAction
{
    public ElementReferenceAction(Guid elementId, string comment)
    {
        ElementId = elementId;
        Comment = comment;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    // ReSharper disable once UnusedMember.Local
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceAction()
    {
        ElementId = Guid.Empty;
        Comment = "";
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    public Guid ElementId { get; set; }
    public string Comment { get; set; }
    public Guid Id { get; private set; }
    public bool UnsavedChanges { get; set; }

    public bool Equals(IAdaptivityAction? other)
    {
        if (other is not ElementReferenceAction elementReferenceAction)
            return false;
        return ElementId.Equals(elementReferenceAction.ElementId) &&
               Id.Equals(elementReferenceAction.Id) &&
               Comment.Equals(elementReferenceAction.Comment);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ElementReferenceAction) obj);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(ElementId, Id, Comment);
    }
    // ReSharper restore NonReadonlyMemberInGetHashCode

    public static bool operator ==(ElementReferenceAction? left, ElementReferenceAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ElementReferenceAction? left, ElementReferenceAction? right)
    {
        return !Equals(left, right);
    }

    public IMemento GetMemento() => new ElementReferenceActionMemento(Id, ElementId, Comment);

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not ElementReferenceActionMemento eram)
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        Id = eram.Id;
        ElementId = eram.ElementId;
        Comment = eram.Comment;
    }

    private record ElementReferenceActionMemento : IMemento
    {
        public ElementReferenceActionMemento(Guid id, Guid elementId, string comment)
        {
            Id = id;
            ElementId = elementId;
            Comment = comment;
        }
        internal Guid Id { get; }
        internal Guid ElementId { get; }
        internal string Comment { get; }
    }
}