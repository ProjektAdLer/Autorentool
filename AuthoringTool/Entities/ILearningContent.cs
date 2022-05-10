namespace AuthoringTool.Entities;

public interface ILearningContent
{
    string Name { get; set; }
    string Type { get; set; }
    byte[] Content { get; set; }
}