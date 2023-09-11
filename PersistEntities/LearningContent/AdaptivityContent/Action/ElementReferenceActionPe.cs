namespace PersistEntities.LearningContent.Action;

public class ElementReferenceActionPe : IAdaptivityActionPe
{
    public ElementReferenceActionPe(Guid elementId)
    {
        ElementId = elementId;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceActionPe()
    {
        ElementId = Guid.Empty;
        Id = Guid.Empty;
    }

    public Guid ElementId { get; set; }
    public Guid Id { get; private set; }
}