using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent;

public class FileContentViewModel : LearningContentViewModel
{
    public FileContentViewModel(string name, string type, string filepath) : base(name)
    {
        Type = type;
        Filepath = filepath;
    }
    
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private FileContentViewModel() : base()
    {
        Type = "";
        Filepath = "";
    }
    
    public string Type { get; set; }
    public string Filepath { get; set; }

}