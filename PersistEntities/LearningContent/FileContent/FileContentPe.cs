using System.Runtime.Serialization;
using JetBrains.Annotations;
using Shared.H5P;

namespace PersistEntities.LearningContent;

[DataContract]
public class FileContentPe : IFileContentPe, IEquatable<FileContentPe>
{
    public FileContentPe(
        string name, 
        string type, 
        string filepath,
        bool isH5P = false,
        H5PContentState h5pState = H5PContentState.NotValidated)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
        IsH5P = isH5P;
        H5PState = h5pState;
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
        IsH5P = false;
        H5PState = H5PContentState.NotValidated;
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
    [DataMember] public bool IsH5P { get; set; }
    [DataMember] public H5PContentState H5PState { get; set; }
}