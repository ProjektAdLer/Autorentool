using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent;

public class LinkContent : LearningContent, ILinkContent
{
    public LinkContent(string name, string link) : base(name)
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

    public string Link { get; set; }
}