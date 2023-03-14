using System.Runtime.Serialization;

namespace PersistEntities.LearningContent;

[DataContract]
public class LinkContentPe : LearningContentPe, ILinkContentPe, IEquatable<LinkContentPe>
{
    public LinkContentPe(string name, string link) : base(name)
    {
        Link = link;
    }

    /// <summary>
    /// Serialization constructor
    /// </summary>
    private LinkContentPe() : base()
    {
        Link = "";
    }
    [DataMember]
    public string Link { get; set; }

    public bool Equals(LinkContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Link == other.Link;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((LinkContentPe)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Link);
    }
}