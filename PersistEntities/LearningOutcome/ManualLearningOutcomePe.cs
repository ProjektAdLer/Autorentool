using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningOutcome;

[Serializable]
[DataContract]
public class ManualLearningOutcomePe : ILearningOutcomePe
{
    public ManualLearningOutcomePe(string outcome)
    {
        Outcome = outcome;
        Id = Guid.NewGuid();
    }

    [UsedImplicitly]
    private ManualLearningOutcomePe()
    {
        Outcome = string.Empty;
        Id = Guid.Empty;
    }

    [DataMember] public string Outcome { get; set; }

    [IgnoreDataMember] public Guid Id { get; set; }

    public string GetOutcome()
    {
        return Outcome;
    }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}