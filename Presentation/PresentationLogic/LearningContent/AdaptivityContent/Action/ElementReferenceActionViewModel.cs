namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class ElementReferenceActionViewModel : IAdaptivityActionViewModel
{
    public ElementReferenceActionViewModel(Guid elementId)
    {
        ElementId = elementId;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceActionViewModel()
    {
        ElementId = Guid.Empty;
        Id = Guid.Empty;
    }

    public Guid ElementId { get; set; }
    public Guid Id { get; private set; }
}