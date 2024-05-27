using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity.Trigger;

[TestFixture]
public class CompositeTriggerUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var condition = ConditionEnum.And;
        var leftTrigger = Substitute.For<IAdaptivityTrigger>();
        var rightTrigger = Substitute.For<IAdaptivityTrigger>();
        var compositeTrigger = new CompositeTrigger(condition, leftTrigger, rightTrigger);

        // Act
        var result1 = compositeTrigger.Equals(null);
        var result2 = compositeTrigger!.Equals((object)null!);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var condition = ConditionEnum.And;
        var leftTrigger = Substitute.For<IAdaptivityTrigger>();
        var rightTrigger = Substitute.For<IAdaptivityTrigger>();
        var compositeTrigger = new CompositeTrigger(condition, leftTrigger, rightTrigger);
        var other = Substitute.For<IAdaptivityTrigger>();

        // Act
        var result1 = compositeTrigger.Equals(other);
        var result2 = compositeTrigger.Equals((object)other);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsCompositeTriggerWithSameValues_ReturnsTrue()
    {
        // Arrange
        var condition = ConditionEnum.And;

        var leftTrigger1 = Substitute.For<IAdaptivityTrigger>();
        leftTrigger1.Equals(Arg.Any<IAdaptivityTrigger>()).Returns(true);

        var rightTrigger1 = Substitute.For<IAdaptivityTrigger>();
        rightTrigger1.Equals(Arg.Any<IAdaptivityTrigger>()).Returns(true);

        var compositeTrigger1 = new CompositeTrigger(condition, leftTrigger1, rightTrigger1);

        var leftTrigger2 = Substitute.For<IAdaptivityTrigger>();
        leftTrigger2.Equals(Arg.Any<IAdaptivityTrigger>()).Returns(true);

        var rightTrigger2 = Substitute.For<IAdaptivityTrigger>();
        rightTrigger2.Equals(Arg.Any<IAdaptivityTrigger>()).Returns(true);

        var compositeTrigger2 = new CompositeTrigger(condition, leftTrigger2, rightTrigger2);

        // Act
        var result = compositeTrigger1.Equals((object?)compositeTrigger2);

        // Assert
        Assert.That(result, Is.True);
    }


    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var condition = ConditionEnum.And;
        var leftTrigger = Substitute.For<IAdaptivityTrigger>();
        var rightTrigger = Substitute.For<IAdaptivityTrigger>();
        var compositeTrigger = new CompositeTrigger(condition, leftTrigger, rightTrigger);
        var expectedHashCode = HashCode.Combine((int)condition, leftTrigger, rightTrigger);

        // Act
        var result = compositeTrigger.GetHashCode();

        // Assert
        Assert.That(expectedHashCode, Is.EqualTo(result));
    }

    [Test]
    public void OperatorEqual_SameReferences_ReturnsTrue()
    {
        // Arrange
        var condition = ConditionEnum.And;
        var leftTrigger = Substitute.For<IAdaptivityTrigger>();
        var rightTrigger = Substitute.For<IAdaptivityTrigger>();
        var compositeTrigger1 = new CompositeTrigger(condition, leftTrigger, rightTrigger);
        var compositeTrigger2 = compositeTrigger1;

        // Act
        var result = compositeTrigger1 == compositeTrigger2;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var condition1 = ConditionEnum.And;
        var condition2 = ConditionEnum.Or;
        var leftTrigger1 = Substitute.For<IAdaptivityTrigger>();
        var rightTrigger1 = Substitute.For<IAdaptivityTrigger>();
        var compositeTrigger1 = new CompositeTrigger(condition1, leftTrigger1, rightTrigger1);

        var leftTrigger2 = Substitute.For<IAdaptivityTrigger>();
        var rightTrigger2 = Substitute.For<IAdaptivityTrigger>();
        var compositeTrigger2 = new CompositeTrigger(condition2, leftTrigger2, rightTrigger2);

        // Act
        var result = compositeTrigger1 != compositeTrigger2;

        // Assert
        Assert.That(result, Is.True);
    }
}