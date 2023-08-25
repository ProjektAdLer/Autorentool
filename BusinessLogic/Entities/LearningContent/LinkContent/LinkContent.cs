using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent.LinkContent;

public class LinkContent : ILinkContent
{
    public LinkContent(string name, string link)
    {
        Name = name;
        Link = link;
    }

    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LinkContent()
    {
        Name = "";
        Link = "";
    }

    public string Name { get; set; }
    public string Link { get; set; }
}