using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningContent.Action;

public class ElementReferenceActionPe : IAdaptivityActionPe
{
    public ElementReferenceActionPe(Guid elementId, string comment)
    {
        ElementId = elementId;
        Comment = comment;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceActionPe()
    {
        ElementId = Guid.Empty;
        Comment = "";
        Id = Guid.Empty;
    }

    [DataMember] public Guid ElementId { get; set; }
    [DataMember] public string Comment { get; set; }
    [IgnoreDataMember] public Guid Id { get; private set; }

    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}