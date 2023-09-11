using JetBrains.Annotations;

namespace PersistEntities.LearningContent;

public class FileContentPe : IFileContentPe, IEquatable<FileContentPe>
{
    public FileContentPe(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }

    /// <summary>
    /// Private constructor for Serialization only.
    /// </summary>
    [UsedImplicitly]
    private FileContentPe()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }

    public bool Equals(FileContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type && Filepath == other.Filepath && Name == other.Name;
    }

    public string Type { get; set; }
    public string Filepath { get; set; }
    public string Name { get; set; }
}