namespace BusinessLogic.Entities.LearningContent.FileContent;

public interface IFileContent : ILearningContent
{
    string Type { get; set; }
    string Filepath { get; set; }
}