namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public class ElementReferenceActionViewModel : IAdaptivityActionViewModel
{
    public ElementReferenceActionViewModel(Guid elementId, string comment)
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
    private ElementReferenceActionViewModel()
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
}