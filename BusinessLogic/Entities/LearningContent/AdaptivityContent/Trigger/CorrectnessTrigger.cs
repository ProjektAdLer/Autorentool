namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Trigger;

/// <summary>
/// Adaptivity trigger that is activated when the user answers a question correctly or incorrectly.
/// </summary>
public class CorrectnessTrigger : IAdaptivityTrigger
{
    public CorrectnessTrigger(AnswerResult expectedAnswer)
    {
        ExpectedAnswer = expectedAnswer;
    }

    public AnswerResult ExpectedAnswer { get; set; }
}