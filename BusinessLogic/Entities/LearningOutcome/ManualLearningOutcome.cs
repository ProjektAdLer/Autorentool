using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningOutcome;

public class ManualLearningOutcome : ILearningOutcome
{
    public ManualLearningOutcome(string outcome)
    {
        Outcome = outcome;
        Id = Guid.NewGuid();
    }

    [UsedImplicitly]
    private ManualLearningOutcome()
    {
        Outcome = string.Empty;
        Id = Guid.Empty;
    }

    public string Outcome { get; set; }

    public Guid Id { get; set; }
}