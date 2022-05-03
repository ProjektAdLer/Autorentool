namespace AuthoringTool.Entities;

public class LearningContent : ILearningContent
{
    internal LearningContent(string name, string type, byte[] content)
    {
        Name = name;
        Type = type;
        Content = content;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    internal LearningContent()
    {
        Name = "";
        Type = "";
        Content = Array.Empty<byte>();
    }
    
    public string Name { get; set; }
    public string Type { get; set; }
    public byte[] Content { get; set; }
}