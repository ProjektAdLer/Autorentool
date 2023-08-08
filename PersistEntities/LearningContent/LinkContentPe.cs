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

    public bool Equals(LinkContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Link == other.Link;
    }

    [DataMember] public string Name { get; set; }
    [DataMember] public string Link { get; set; }
}