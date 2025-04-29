using BusinessLogic.Entities.LearningContent.H5P;
using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent.FileContent;

public class FileContentViewModel : IFileContentViewModel
{
    public FileContentViewModel(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
        UnsavedChanges = true;
        IsH5P = false;
        H5PState = H5PContentState.Unknown;
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
        UnsavedChanges = false;
        IsH5P = false;
        H5PState = H5PContentState.Unknown;
    }
    
    public string Name { get; init; }
    public bool UnsavedChanges { get; set; }
    public string Type { get; init; }
    public string Filepath { get; init; }
    public bool IsH5P { get; set; }
    public H5PContentState H5PState { get; set; }

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