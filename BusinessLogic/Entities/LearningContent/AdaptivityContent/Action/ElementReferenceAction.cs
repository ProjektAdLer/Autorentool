namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

public class ElementReferenceAction : IAdaptivityAction
{
    public ElementReferenceAction(Guid elementId)
    {
        ElementId = elementId;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceAction()
    {
        ElementId = Guid.Empty;
    }
    
    public Guid ElementId { get; set; }
}