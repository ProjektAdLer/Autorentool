namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class ElementReferenceActionViewModel : IAdaptivityActionViewModel
{
    public ElementReferenceActionViewModel(Guid elementId)
    {
        ElementId = elementId;
    }
    
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceActionViewModel()
    {
        ElementId = Guid.Empty;
    }

    public Guid ElementId { get; set; }
}