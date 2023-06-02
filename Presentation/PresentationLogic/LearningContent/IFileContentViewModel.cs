namespace Presentation.PresentationLogic.LearningContent;

public interface IFileContentViewModel : ILearningContentViewModel
{
    string Type { get; set; }
    string Filepath { get; set; }
}