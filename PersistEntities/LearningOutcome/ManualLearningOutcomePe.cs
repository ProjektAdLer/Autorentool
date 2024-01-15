namespace PersistEntities.LearningOutcome;

public class ManualLearningOutcomePe : ILearningOutcomePe
{
    private readonly string _outcome;

    public ManualLearningOutcomePe(string outcome)
    {
        _outcome = outcome;
    }
}