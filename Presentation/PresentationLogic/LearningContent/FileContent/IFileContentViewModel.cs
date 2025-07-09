using Shared.H5P;

namespace Presentation.PresentationLogic.LearningContent.FileContent;

public interface IFileContentViewModel : ILearningContentViewModel
{
    string Type { get; init; }
    string Filepath { get; init; }
    bool IsH5P { get; set; }
    H5PContentState H5PState { get; set; }
}