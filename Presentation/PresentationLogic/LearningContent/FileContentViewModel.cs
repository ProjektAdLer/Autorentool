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
    
    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }

}