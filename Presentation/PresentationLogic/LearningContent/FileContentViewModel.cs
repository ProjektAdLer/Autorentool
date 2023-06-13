using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent;

public class FileContentViewModel : IFileContentViewModel
{
    public FileContentViewModel(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }
    
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private FileContentViewModel()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }
    
    public string Name { get; init; }
    public string Type { get; init; }
    public string Filepath { get; init; }

    protected bool Equals(FileContentViewModel other) => Name == other.Name && Type == other.Type && Filepath == other.Filepath;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is FileContentViewModel fileContentViewModel && Equals(fileContentViewModel);
    }

    public override int GetHashCode() => HashCode.Combine(Name, Type, Filepath);

    public override string ToString() => Name;
}