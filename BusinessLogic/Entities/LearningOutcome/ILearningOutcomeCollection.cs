namespace BusinessLogic.Entities.LearningOutcome;

public interface ILearningOutcomeCollection
{
    List<ILearningOutcome> LearningOutcomes { get; set; }
    bool InternalUnsavedChanges { get; }
    bool UnsavedChanges { get; set; }
}