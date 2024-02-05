namespace Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

public interface ILearningOutcomeViewModel
{
    Guid Id { get; set; }
    string GetOutcome();
}