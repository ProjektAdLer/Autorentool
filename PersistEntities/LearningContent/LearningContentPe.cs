using System.Runtime.Serialization;

namespace PersistEntities.LearningContent;

[DataContract]
[KnownType(typeof(FileContentPe))]
[KnownType(typeof(LinkContentPe))]
public abstract class LearningContentPe : ILearningContentPe, IEquatable<LearningContentPe>
{
    protected LearningContentPe(string name)
    {
        Name = name;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    protected LearningContentPe()
    {
        Name = "";
    }
    
    [DataMember]
    public string Name { get; set; }

    public bool Equals(LearningContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((LearningContentPe)obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}