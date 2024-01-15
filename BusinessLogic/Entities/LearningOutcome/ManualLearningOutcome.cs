namespace BusinessLogic.Entities.LearningOutcome;

public class ManualLearningOutcome : ILearningOutcome
{
    private readonly string _outcome;

    public ManualLearningOutcome(string outcome)
    {
        _outcome = outcome;
    }
}