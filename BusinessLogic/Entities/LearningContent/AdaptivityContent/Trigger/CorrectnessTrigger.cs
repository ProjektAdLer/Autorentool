using Shared;
using Shared.Adaptivity;

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

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CorrectnessTrigger()
    {
        ExpectedAnswer = AnswerResult.Correct;
    }

    /// <summary>
    /// Whether the answer must be correct or incorrect for the trigger to be activated.
    /// </summary>
    public AnswerResult ExpectedAnswer { get; set; }
}