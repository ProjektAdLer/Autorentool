namespace PersistEntities;

public class ContentPe : IContentPe
{
    public ContentPe(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    internal ContentPe()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }
    
    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
}