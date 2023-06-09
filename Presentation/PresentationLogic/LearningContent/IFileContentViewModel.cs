namespace Presentation.PresentationLogic.LearningContent;

public interface IFileContentViewModel : ILearningContentViewModel
{
    string Type { get; init; }
    string Filepath { get; init; }
}