using JetBrains.Annotations;

namespace PersistEntities.LearningOutcome;

public class LearningOutcomeCollectionPe
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

    public List<ILearningOutcomePe> LearningOutcomes { get; set; }
}