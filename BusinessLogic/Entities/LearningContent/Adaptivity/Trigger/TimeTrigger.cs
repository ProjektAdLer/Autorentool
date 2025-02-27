using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;

/// <summary>
/// Adaptivity trigger that is activated when the user spends a certain amount of time on a question.
/// </summary>
public class TimeTrigger : IAdaptivityTrigger
{
    public TimeTrigger(int expected, TimeFrameType timeFrameType)
    {
        Expected = expected;
        TimeFrameType = timeFrameType;
        UnsavedChanges = true;
    }

    // ReSharper disable once UnusedMember.Local
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private TimeTrigger()
    {
        Expected = 0;
        TimeFrameType = TimeFrameType.From;
        UnsavedChanges = false;
    }

    /// <summary>
    /// The time in seconds that the user must spend on the question for the trigger to be activated.
    /// </summary>
    public int Expected { get; set; }

    /// <summary>
    /// Whether the user must spend less or more than <see cref="Expected"/> seconds for the trigger to be activated.
    /// </summary>
    public TimeFrameType TimeFrameType { get; set; }
    
    public bool UnsavedChanges { get; set; }

    public bool Equals(IAdaptivityTrigger? other)
    {
        if (other is not TimeTrigger timeTrigger)
            return false;
        return Expected == timeTrigger.Expected && TimeFrameType == timeTrigger.TimeFrameType;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TimeTrigger) obj);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Expected, (int) TimeFrameType);
    }
    // ReSharper restore NonReadonlyMemberInGetHashCode

    public static bool operator ==(TimeTrigger? left, TimeTrigger? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TimeTrigger? left, TimeTrigger? right)
    {
        return !Equals(left, right);
    }

}