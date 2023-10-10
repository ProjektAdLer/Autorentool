using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningContent.Question;

public class ChoicePe
{
    public ChoicePe(string text)
    {
        Text = text;
        Id = Guid.NewGuid();
    }

    private ChoicePe()
    {
        Text = "";
        Id = Guid.Empty;
    }

    [DataMember]
    public string Text { get; set; }
    [IgnoreDataMember]
    public Guid Id { get; set; }
    
    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}