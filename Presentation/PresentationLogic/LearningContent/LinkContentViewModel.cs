using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent;

public class LinkContentViewModel : LearningContentViewModel, ILinkContentViewModel
{
    public LinkContentViewModel(string name, string link) : base(name)
    {
        Link = link;
    }
    
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private LinkContentViewModel() : base()
    {
        Link = "";
    }
    
    public string Link { get; set; }
}