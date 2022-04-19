namespace AuthoringTool.Entities;

public interface ILearningContent
{
    string Name { get; set; }
    string Type { get; set; }
    string Content { get; set; }
}