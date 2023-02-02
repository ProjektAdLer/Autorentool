namespace PersistEntities;

public class PathwayPe: IPathWayPe
{
    protected PathwayPe()
    {
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards. - m.ho
        SourceObject = null!;
        TargetObject = null!;
    }
    
    public PathwayPe(IObjectInPathWayPe sourceObject, IObjectInPathWayPe targetObject)
    {
        SourceObject = sourceObject;
        TargetObject = targetObject;
    }
    
    public IObjectInPathWayPe SourceObject { get; set; }
    public IObjectInPathWayPe TargetObject { get; set; }
}