using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent;

public class LinkContentViewModel : ILinkContentViewModel
{
    public LinkContentViewModel(string name, string link)
    {
        Name = name;
        Link = link;
    }
    
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private LinkContentViewModel()
    {
        Name = "";
        Link = "";
    }
    
    public string Name { get; set; }
    public string Link { get; set; }
}