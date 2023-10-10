using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent.LinkContent;

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
    
    public string Name { get; init; }
    public string Link { get; init; }
    
    protected bool Equals(LinkContentViewModel other) => Name == other.Name && Link == other.Link;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is LinkContentViewModel linkContentViewModel && Equals(linkContentViewModel);
    }

    public override int GetHashCode() => HashCode.Combine(Name, Link);

    public override string ToString() => Name;
}