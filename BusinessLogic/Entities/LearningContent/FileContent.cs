using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent;

public class FileContent : LearningContent, IFileContent
{
    public FileContent(string name, string type, string filepath) : base(name)
    {
        Type = type;
        Filepath = filepath;
    }

    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private FileContent() : base()
    {
        Type = "";
        Filepath = "";
    }

    public string Type { get; set; }
    public string Filepath { get; set; }
}