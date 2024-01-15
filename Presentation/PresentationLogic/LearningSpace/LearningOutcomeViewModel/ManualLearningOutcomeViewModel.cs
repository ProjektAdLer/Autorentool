namespace Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

public class ManualLearningOutcomeViewModel : ILearningOutcomeViewModel
{
    private readonly string _outcome;

    public ManualLearningOutcomeViewModel(string outcome)
    {
        _outcome = outcome;
    }

    public string GetOutcome()
    {
        return _outcome;
    }
}