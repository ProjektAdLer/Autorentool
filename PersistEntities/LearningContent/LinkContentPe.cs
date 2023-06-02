using System.Runtime.Serialization;

namespace PersistEntities.LearningContent;

[DataContract]
public class LinkContentPe : ILinkContentPe, IEquatable<LinkContentPe>
{
    public LinkContentPe(string name, string link)
    {
        Name = name;
        Link = link;
    }

    /// <summary>
    /// Serialization constructor
    /// </summary>
    private LinkContentPe()
    {
        Name = "";
        Link = "";
    }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Link { get; set; }

    public bool Equals(LinkContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Link == other.Link;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LinkContentPe)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Link);
    }
}