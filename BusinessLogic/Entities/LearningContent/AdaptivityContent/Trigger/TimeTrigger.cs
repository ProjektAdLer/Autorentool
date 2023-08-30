using Shared;
using Shared.Adaptivity;

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

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private TimeTrigger()
    {
        Expected = 0;
        TimeFrameType = TimeFrameType.From;
    }

    /// <summary>
    /// The time in seconds that the user must spend on the question for the trigger to be activated.
    /// </summary>
    public int Expected { get; set; }
    /// <summary>
    /// Whether the user must spend less or more than <see cref="Expected"/> seconds for the trigger to be activated.
    /// </summary>
    public TimeFrameType TimeFrameType { get; set; }
}