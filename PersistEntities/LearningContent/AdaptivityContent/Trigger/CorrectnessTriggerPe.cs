using System.Runtime.Serialization;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent.Trigger;

/// <summary>
/// Adaptivity trigger that is activated when the user answers a question correctly or incorrectly.
/// </summary>
public class CorrectnessTriggerPe : IAdaptivityTriggerPe
{
    public CorrectnessTriggerPe(AnswerResult expectedAnswer)
    {
        ExpectedAnswer = expectedAnswer;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private CorrectnessTriggerPe()
    {
        ExpectedAnswer = AnswerResult.Correct;
    }

    /// <summary>
    /// Whether the answer must be correct or incorrect for the trigger to be activated.
    /// </summary>
    [DataMember]
    public AnswerResult ExpectedAnswer { get; set; }
}