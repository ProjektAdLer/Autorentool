namespace BusinessLogic.Entities;

public class LearningContent : ILearningContent
{
    public LearningContent(string name, string type, byte[] content)
    {
        Name = name;
        Type = type;
        Content = content;
    }
    
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
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