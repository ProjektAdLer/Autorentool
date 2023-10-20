namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public class ElementReferenceAction : IAdaptivityAction
{
    public ElementReferenceAction(Guid elementId, string comment)
    {
        ElementId = elementId;
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceAction()
    {
        ElementId = Guid.Empty;
        Comment = "";
        Id = Guid.Empty;
    }

    public Guid ElementId { get; set; }
    public string Comment { get; set; }
    public Guid Id { get; private set; }

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

    public override int GetHashCode()
    {
        return HashCode.Combine(ElementId, Id, Comment);
    }

    public static bool operator ==(ElementReferenceAction? left, ElementReferenceAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ElementReferenceAction? left, ElementReferenceAction? right)
    {
        return !Equals(left, right);
    }
}