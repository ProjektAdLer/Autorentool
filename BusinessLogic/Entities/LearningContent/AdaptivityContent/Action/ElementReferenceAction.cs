namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

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
}