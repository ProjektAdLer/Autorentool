using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
public class LearningPathwayPe : ILearningPathWayPe
{
    protected LearningPathwayPe()
    {
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards. - m.ho
        SourceObject = null!;
        TargetObject = null!;
    }

    public LearningPathwayPe(IObjectInPathWayPe sourceObject, IObjectInPathWayPe targetObject)
    {
        SourceObject = sourceObject;
        TargetObject = targetObject;
    }

    [IgnoreDataMember] public Guid Id { get; set; }

    [DataMember] public IObjectInPathWayPe SourceObject { get; set; }

    [DataMember] public IObjectInPathWayPe TargetObject { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}