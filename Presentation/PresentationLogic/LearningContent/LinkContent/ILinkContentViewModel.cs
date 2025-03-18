namespace Presentation.PresentationLogic.LearningContent.LinkContent;

public interface ILinkContentViewModel : ILearningContentViewModel
{
    string Link { get; init; }
    bool IsDeleted { get; set; }
}