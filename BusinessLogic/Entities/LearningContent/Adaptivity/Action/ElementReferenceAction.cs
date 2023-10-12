namespace BusinessLogic.Entities.LearningContent.Adaptivity.Action;

public class ElementReferenceAction : IAdaptivityAction
{
    public ElementReferenceAction(Guid elementId)
    {
        ElementId = elementId;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceAction()
    {
        ElementId = Guid.Empty;
        Id = Guid.Empty;
    }

    public Guid ElementId { get; set; }
    public Guid Id { get; private set; }

    public bool Equals(IAdaptivityAction? other)
    {
        if (other is not ElementReferenceAction elementReferenceAction)
            return false;
        return ElementId.Equals(elementReferenceAction.ElementId) && Id.Equals(elementReferenceAction.Id);
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
        return HashCode.Combine(ElementId, Id);
    }

    public static bool operator ==(ElementReferenceAction? left, ElementReferenceAction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ElementReferenceAction? left, ElementReferenceAction? right)
    {
        return !Equals(left, right);
    }

    public IMemento GetMemento() => new ElementReferenceActionMemento(Id, ElementId);

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not ElementReferenceActionMemento eram)
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        Id = eram.Id;
        ElementId = eram.ElementId;
    }

    private record ElementReferenceActionMemento : IMemento
    {
        public ElementReferenceActionMemento(Guid id, Guid elementId)
        {
            Id = id;
            ElementId = elementId;
        }
        internal Guid Id { get; }
        internal Guid ElementId { get; }
    }
}