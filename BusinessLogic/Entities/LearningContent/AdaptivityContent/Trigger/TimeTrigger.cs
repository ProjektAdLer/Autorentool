namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Trigger;

/// <summary>
/// Adaptivity trigger that is activated when the user spends a certain amount of time on a question.
/// </summary>
public class TimeTrigger : IAdaptivityTrigger
{
    public TimeTrigger(int expected, TimeFrameType timeFrameType)
    {
        Expected = expected;
        TimeFrameType = timeFrameType;
    }

    public int Expected { get; set; }
    public TimeFrameType TimeFrameType { get; set; }
}