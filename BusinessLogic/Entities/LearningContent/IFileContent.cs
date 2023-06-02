namespace BusinessLogic.Entities.LearningContent;

public interface IFileContent : ILearningContent 
{
    string Type { get; set; }
    string Filepath { get; set; }
}