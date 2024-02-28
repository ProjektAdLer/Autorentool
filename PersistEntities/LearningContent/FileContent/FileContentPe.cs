using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningContent;

public class FileContentPe : IFileContentPe, IEquatable<FileContentPe>
{
    public FileContentPe(string name, string type, string filepath, bool primitiveH5P = false)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
        PrimitiveH5P = primitiveH5P;
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
        PrimitiveH5P = false;
    }

    public bool Equals(FileContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type && Filepath == other.Filepath && Name == other.Name;
    }

    [DataMember] public string Type { get; set; }
    [DataMember] public string Filepath { get; set; }
    [DataMember] public string Name { get; set; }
    [DataMember] public bool PrimitiveH5P { get; set; }
}