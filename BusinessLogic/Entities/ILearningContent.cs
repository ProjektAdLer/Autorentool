namespace BusinessLogic.Entities;

public interface ILearningContent
{
    string Name { get; set; }
    string Type { get; set; }
    string Filepath { get; set; }
}