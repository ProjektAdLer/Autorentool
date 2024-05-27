using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using NUnit.Framework;
using Shared.Adaptivity;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity.Trigger;

[TestFixture]
public class TimeTriggerUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var timeTrigger = new TimeTrigger(10, TimeFrameType.From);

        // Act
        var result1 = timeTrigger.Equals(null);
        var result2 = timeTrigger!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var timeTrigger = new TimeTrigger(10, TimeFrameType.From);

        // Act
        var result1 = timeTrigger.Equals(timeTrigger);
        var result2 = timeTrigger.Equals((object)timeTrigger);

        // Assert
        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var timeTrigger = new TimeTrigger(10, TimeFrameType.From);
        var other = new object();

        // Act
        var result = timeTrigger.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsTimeTriggerWithSameValues_ReturnsTrue()
    {
        // Arrange
        var timeTrigger1 = new TimeTrigger(10, TimeFrameType.From);
        var timeTrigger2 = new TimeTrigger(10, TimeFrameType.From);

        // Act
        var result = timeTrigger1.Equals((object?)timeTrigger2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsTimeTriggerWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var timeTrigger1 = new TimeTrigger(10, TimeFrameType.From);
        var timeTrigger2 = new TimeTrigger(20, TimeFrameType.Until);

        // Act
        var result = timeTrigger1.Equals((object?)timeTrigger2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var timeTrigger = new TimeTrigger(10, TimeFrameType.From);
        var expectedHashCode = HashCode.Combine(10, (int)TimeFrameType.From);

        // Act
        var result = timeTrigger.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsTrue()
    {
        // Arrange
        var timeTrigger1 = new TimeTrigger(10, TimeFrameType.From);
        var timeTrigger2 = new TimeTrigger(10, TimeFrameType.From);

        // Act
        var result = timeTrigger1 == timeTrigger2;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var timeTrigger1 = new TimeTrigger(10, TimeFrameType.From);
        var timeTrigger2 = new TimeTrigger(20, TimeFrameType.Until);

        // Act
        var result = timeTrigger1 != timeTrigger2;

        // Assert
        Assert.That(result, Is.True);
    }
}