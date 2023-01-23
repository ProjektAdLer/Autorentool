using System.Runtime.Serialization;

namespace PersistEntities;

public class TopicPe
{
    public TopicPe(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    protected TopicPe()
    {
        Id = Guid.Empty;
        Name = string.Empty;
    }
    
    [DataMember]
    public string Name { get; set; }
    [IgnoreDataMember]
    public Guid Id { get; set; }
    
    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}