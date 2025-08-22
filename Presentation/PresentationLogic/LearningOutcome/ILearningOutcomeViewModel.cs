namespace Presentation.PresentationLogic.LearningOutcome;

public interface ILearningOutcomeViewModel
{
    Guid Id { get; set; }
    string GetOutcome();
}