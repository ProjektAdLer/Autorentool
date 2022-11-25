using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class LearningPathway : ILearningPathway
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    protected LearningPathway()
    {
        Id = Guid.Empty;
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards. - m.ho
        SourceObject = null!;
        TargetObject = null!;
    }
    
    public LearningPathway(IObjectInPathWay sourceObject, IObjectInPathWay targetObject)
    {
        Id = Guid.NewGuid();
        SourceObject = sourceObject;
        TargetObject = targetObject;
    }
    
    public IObjectInPathWay SourceObject { get; set; }
    public IObjectInPathWay TargetObject { get; set; }
    public Guid Id { get; private set; }
}