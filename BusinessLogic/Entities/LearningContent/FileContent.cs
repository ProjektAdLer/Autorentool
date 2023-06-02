using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent;

public class FileContent : IFileContent
{
    public FileContent(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }

    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private FileContent()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
}