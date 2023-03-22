using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent;

public class LinkContent : ILinkContent
{
    public LinkContent(string name, string link)
    {
        Link = link;
    }
    
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LinkContent() : base()
    {
        Link = "";
    }

    public string Name { get; set; }
    public string Link { get; set; }
}