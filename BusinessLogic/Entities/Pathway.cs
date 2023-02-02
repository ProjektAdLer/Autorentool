using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class Pathway : IPathway
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    protected Pathway()
    {
        Id = Guid.Empty;
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards. - m.ho
        SourceObject = null!;
        TargetObject = null!;
    }
    
    public Pathway(IObjectInPathWay sourceObject, IObjectInPathWay targetObject)
    {
        Id = Guid.NewGuid();
        SourceObject = sourceObject;
        TargetObject = targetObject;
    }
    
    public IObjectInPathWay SourceObject { get; set; }
    public IObjectInPathWay TargetObject { get; set; }
    public Guid Id { get; private set; }
}