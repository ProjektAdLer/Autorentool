namespace AuthoringTool.Entities;

public class LearningContent : ILearningContent
{
    internal LearningContent(string name, string type, byte[] content)
    {
        Name = name;
        Type = type;
        Content = content;
    }
    
    public string Name { get; set; }
    public string Type { get; set; }
    public byte[] Content { get; set; }
}