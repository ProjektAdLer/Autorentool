using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities.LearningOutcome;

[Serializable]
[DataContract]
[KnownType(typeof(ManualLearningOutcomePe))]
[KnownType(typeof(StructuredLearningOutcomePe))]
public class LearningOutcomeCollectionPe : ILearningOutcomeCollectionPe
{
    public LearningOutcomeCollectionPe(List<ILearningOutcomePe>? learningOutcomes = null)
    {
        LearningOutcomes = learningOutcomes ?? new List<ILearningOutcomePe>();
    }

    [UsedImplicitly]
    private LearningOutcomeCollectionPe()
    {
        LearningOutcomes = new List<ILearningOutcomePe>();
    }

    [DataMember] public List<ILearningOutcomePe> LearningOutcomes { get; set; }
}