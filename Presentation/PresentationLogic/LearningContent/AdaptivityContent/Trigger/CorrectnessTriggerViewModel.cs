using Shared;
using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;

/// <summary>
/// Adaptivity trigger that is activated when the user answers a question correctly or incorrectly.
/// </summary>
public class CorrectnessTriggerViewModel : IAdaptivityTriggerViewModel
{
    public CorrectnessTriggerViewModel(AnswerResult expectedAnswer)
    {
        ExpectedAnswer = expectedAnswer;
    }

    /// <summary>
    /// Whether the answer must be correct or incorrect for the trigger to be activated.
    /// </summary>
    public AnswerResult ExpectedAnswer { get; set; }
}