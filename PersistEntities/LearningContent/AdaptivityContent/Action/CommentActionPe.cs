using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningContent.Action;

public class CommentActionPe : IAdaptivityActionPe
{
    public CommentActionPe(string comment)
    {
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CommentActionPe()
    {
        Comment = "";
        Id = Guid.Empty;
    }

    [DataMember] public string Comment { get; set; }
    [IgnoreDataMember] public Guid Id { get; private set; }

    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}