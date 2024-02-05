using JetBrains.Annotations;

namespace PersistEntities.LearningOutcome;

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

    public string Outcome { get; set; }

    public Guid Id { get; set; }

    public string GetOutcome()
    {
        return Outcome;
    }
}