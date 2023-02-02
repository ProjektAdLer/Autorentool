using JetBrains.Annotations;

namespace Presentation.PresentationLogic.Content;

public class ContentViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private ContentViewModel()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the  content</param>
    /// <param name="type">Describes the type of the loaded file</param>
    /// <param name="filepath">Contains the content of the loaded file</param>
    public ContentViewModel(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
    
}