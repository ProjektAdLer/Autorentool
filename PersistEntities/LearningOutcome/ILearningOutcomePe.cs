namespace PersistEntities.LearningOutcome;

public interface ILearningOutcomePe
{
    Guid Id { get; set; }
    string GetOutcome();
}