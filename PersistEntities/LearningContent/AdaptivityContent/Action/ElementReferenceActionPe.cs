using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningContent.Action;

public class ElementReferenceActionPe : IAdaptivityActionPe
{
    public ElementReferenceActionPe(Guid elementId)
    {
        ElementId = elementId;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private ElementReferenceActionPe()
    {
        ElementId = Guid.Empty;
        Id = Guid.Empty;
    }

    [DataMember] public Guid ElementId { get; set; }
    [IgnoreDataMember] public Guid Id { get; private set; }

    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}