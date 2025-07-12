using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningOutcome;

public class ManualLearningOutcomeViewModel : ILearningOutcomeViewModel
{
    [UsedImplicitly]
    private ManualLearningOutcomeViewModel()
    {
        Outcome = string.Empty;
        Id = Guid.Empty;
    }

    public ManualLearningOutcomeViewModel(string outcome)
    {
        Outcome = outcome;
        Id = Guid.NewGuid();
    }

    public string Outcome { get; set; }

    public Guid Id { get; set; }

    public string GetOutcome()
    {
        return Outcome;
    }
}