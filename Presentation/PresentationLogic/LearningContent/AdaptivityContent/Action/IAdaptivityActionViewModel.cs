namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;

public interface IAdaptivityActionViewModel
{
    public Guid Id { get; }
    bool UnsavedChanges { get; set; }
}