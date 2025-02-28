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

    // ReSharper disable once UnusedMember.Local
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CorrectnessTriggerViewModel()
    {
        ExpectedAnswer = AnswerResult.Correct;
    }

    /// <summary>
    /// Whether the answer must be correct or incorrect for the trigger to be activated.
    /// </summary>
    public AnswerResult ExpectedAnswer { get; set; }

    public bool UnsavedChanges { get; set; }
}