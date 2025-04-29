namespace Presentation.PresentationLogic.LearningContent.FileContent;

public interface IFileContentViewModel : ILearningContentViewModel
{
    string Type { get; init; }
    string Filepath { get; init; }
    bool IsH5P { get; set; }
}