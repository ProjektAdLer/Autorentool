using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent.LinkContent;

public class LinkContent : ILinkContent
{
    public LinkContent(string name, string link)
    {
        Name = name;
        Link = link;
        UnsavedChanges = true;
    }

    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LinkContent()
    {
        Name = "";
        Link = "";
        UnsavedChanges = false;
    }

    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public string Link { get; set; }

    public bool Equals(ILearningContent? other)
    {
        if(other is not LinkContent linkContent) return false;
        return Name == linkContent.Name && Link == linkContent.Link;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LinkContent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Link);
    }

    public static bool operator ==(LinkContent? left, LinkContent? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(LinkContent? left, LinkContent? right)
    {
        return !Equals(left, right);
    }
}