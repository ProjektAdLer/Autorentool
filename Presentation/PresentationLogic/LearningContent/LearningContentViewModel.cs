namespace Presentation.PresentationLogic.LearningContent;

public class LearningContentViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LearningContentViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning content</param>
    /// <param name="type">Describes the type of the loaded file</param>
    /// <param name="content">Contains the content of the loaded file</param>
    public LearningContentViewModel(string name, string type, byte[] content)
    {
        Name = name;
        Type = type;
        Content = content;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public byte[] Content { get; set; }
    
}