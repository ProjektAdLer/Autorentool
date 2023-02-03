namespace PersistEntities;

public class LearningPathwayPe: ILearningPathWayPe
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
    
    public IObjectInPathWayPe SourceObject { get; set; }
    public IObjectInPathWayPe TargetObject { get; set; }
}