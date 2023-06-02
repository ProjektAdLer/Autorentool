namespace PersistEntities.LearningContent;

public interface IFileContentPe : ILearningContentPe
{
    string Type { get; set; }
    string Filepath { get; set; }
}