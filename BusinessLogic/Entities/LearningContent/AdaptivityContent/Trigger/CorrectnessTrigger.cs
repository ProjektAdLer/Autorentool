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

    public bool Equals(IAdaptivityTrigger? other)
    {
        if(other is not CorrectnessTrigger correctnessTrigger)
            return false;
        return ExpectedAnswer == correctnessTrigger.ExpectedAnswer;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CorrectnessTrigger)obj);
    }

    public override int GetHashCode()
    {
        return (int)ExpectedAnswer;
    }

    public static bool operator ==(CorrectnessTrigger? left, CorrectnessTrigger? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CorrectnessTrigger? left, CorrectnessTrigger? right)
    {
        return !Equals(left, right);
    }
}