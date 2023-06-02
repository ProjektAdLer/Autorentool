namespace PersistEntities.LearningContent;

public class FileContentPe : IFileContentPe, IEquatable<FileContentPe>
{
    public FileContentPe(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }

    private FileContentPe()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }

    public string Type { get; set; }
    public string Filepath { get; set; }
    public string Name { get; set; }

    public bool Equals(FileContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type && Filepath == other.Filepath && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FileContentPe)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Filepath, Name);
    }
}