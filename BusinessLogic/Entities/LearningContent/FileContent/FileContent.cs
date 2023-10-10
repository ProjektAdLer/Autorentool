using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent.FileContent;

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

    public bool Equals(ILearningContent? other)
    {
        if (other is not IFileContent fileContent) return false;
        return Name == fileContent.Name && Type == fileContent.Type && Filepath == fileContent.Filepath;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(FileContent)) return false;
        return Equals((FileContent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Type, Filepath);
    }

    public static bool operator ==(FileContent? left, FileContent? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FileContent? left, FileContent? right)
    {
        return !Equals(left, right);
    }
}