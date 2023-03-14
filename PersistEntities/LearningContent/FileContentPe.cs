namespace PersistEntities.LearningContent;

public class FileContentPe : LearningContentPe, IFileContentPe
{
    public FileContentPe(string name, string type, string filepath) : base(name)
    {
        Type = type;
        Filepath = filepath;
    }

    private FileContentPe() : base()
    {
        Type = "";
        Filepath = "";
    }

    public string Type { get; set; }
    public string Filepath { get; set; }
}