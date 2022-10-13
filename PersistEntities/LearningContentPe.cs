namespace PersistEntities;

public class LearningContentPe : ILearningContentPe
{
    public LearningContentPe(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    internal LearningContentPe()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }
    
    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
}