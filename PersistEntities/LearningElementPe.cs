using System.Runtime.Serialization;
using PersistEntities.LearningContent;
using PersistEntities.LearningContent.Story;
using Shared;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(FileContentPe))]
[KnownType(typeof(LinkContentPe))]
[KnownType(typeof(AdaptivityContentPe))]
[KnownType(typeof(StoryContentPe))]
public class LearningElementPe : ILearningElementPe, IExtensibleDataObject
{
    public LearningElementPe(string name, ILearningContentPe learningContent,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload = 0,
        int points = 1, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        LearningContent = learningContent;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        ElementModel = elementModel;
        Workload = workload;
        Points = points;
        PositionX = positionX;
        PositionY = positionY;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    protected LearningElementPe()
    {
        Id = Guid.Empty;
        Name = "";
        // Overridden because private automapper/serialization constructor - n.stich
        LearningContent = null!;
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.Medium;
        ElementModel = ElementModel.l_h5p_slotmachine_1;
        Workload = 0;
        Points = 1;
        PositionX = 0;
        PositionY = 0;
    }

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [DataMember(IsRequired = false)] public Guid Id { get; set; }

    [DataMember] public string Name { get; set; }

    [DataMember] public ILearningContentPe LearningContent { get; set; }

    [DataMember] public string Description { get; set; }

    [DataMember] public string Goals { get; set; }

    [DataMember] public int Workload { get; set; }

    [DataMember] public int Points { get; set; }

    [DataMember] public LearningElementDifficultyEnum Difficulty { get; set; }

    [DataMember] public ElementModel ElementModel { get; set; }

    [DataMember] public double PositionX { get; set; }

    [DataMember] public double PositionY { get; set; }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        if (Id == Guid.Empty)
            Id = Guid.NewGuid();
        if (IsObsolete(ElementModel))
        {
            ElementModel = GetAlternateValue(ElementModel);
        }
    }
    
    private static bool IsObsolete(ElementModel model)
    {
        var memberInfo = typeof(ElementModel).GetMember(model.ToString()).FirstOrDefault();
        return memberInfo?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0;
    }
    
    private static ElementModel GetAlternateValue(ElementModel model)
    {
        var memberInfo = typeof(ElementModel).GetMember(model.ToString()).FirstOrDefault();
        var alternateValue = memberInfo?.GetCustomAttributes(typeof(AlternateValueAttribute), false).FirstOrDefault();
        
        return alternateValue != null ?
            (ElementModel) ((AlternateValueAttribute) alternateValue).AlternateValue :
            ElementModel.a_npc_defaultdark_female;
    }
    
    
}