using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent.LinkContent;

public class LinkContent : ILinkContent
{
    public LinkContent(string name, string link)
    {
        Name = name;
        Link = link;
        UnsavedChanges = true;
        IsDeleted = false;
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
        IsDeleted = false;
    }

    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public string Link { get; set; }
    public bool IsDeleted { get; set; }

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

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Link);
    }
    // ReSharper restore NonReadonlyMemberInGetHashCode

    public static bool operator ==(LinkContent? left, LinkContent? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(LinkContent? left, LinkContent? right)
    {
        return !Equals(left, right);
    }
}