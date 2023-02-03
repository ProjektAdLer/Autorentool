using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent;

public class LearningContentViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private LearningContentViewModel()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningContentViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning content</param>
    /// <param name="type">Describes the type of the loaded file</param>
    /// <param name="filepath">Contains the content of the loaded file</param>
    public LearningContentViewModel(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
    
}