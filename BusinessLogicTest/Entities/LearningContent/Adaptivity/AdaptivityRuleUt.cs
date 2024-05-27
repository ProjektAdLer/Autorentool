using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity;

[TestFixture]
public class AdaptivityRuleUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var rule = CreateSampleRule();

        // Act
        var result1 = rule.Equals(null);
        var result2 = rule!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var rule = CreateSampleRule();

        // Act
        var result = rule.Equals((object)rule);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var rule = CreateSampleRule();
        var other = new object();

        // Act
        var result = rule.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsAdaptivityRuleWithSameValues_ReturnsFalse()
    {
        // Arrange
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();

        var rule1 = new AdaptivityRule(trigger, action);
        var rule2 = new AdaptivityRule(trigger, action);

        // Act
        var result = rule1.Equals((object?)rule2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsAdaptivityRuleWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var trigger1 = Substitute.For<IAdaptivityTrigger>();
        var action1 = Substitute.For<IAdaptivityAction>();

        var trigger2 = Substitute.For<IAdaptivityTrigger>();
        var action2 = Substitute.For<IAdaptivityAction>();

        var rule1 = new AdaptivityRule(trigger1, action1);
        var rule2 = new AdaptivityRule(trigger2, action2);

        // Act
        var result = rule1.Equals((object?)rule2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var rule = CreateSampleRule();
        var expectedHashCode = HashCode.Combine(rule.Trigger, rule.Action);

        // Act
        var result = rule.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsFalse()
    {
        // Arrange
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();

        var rule1 = new AdaptivityRule(trigger, action);
        var rule2 = new AdaptivityRule(trigger, action);

        // Act
        var result = rule1 == rule2;

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var trigger1 = Substitute.For<IAdaptivityTrigger>();
        var action1 = Substitute.For<IAdaptivityAction>();

        var trigger2 = Substitute.For<IAdaptivityTrigger>();
        var action2 = Substitute.For<IAdaptivityAction>();

        var rule1 = new AdaptivityRule(trigger1, action1);
        var rule2 = new AdaptivityRule(trigger2, action2);

        // Act
        var result = rule1 != rule2;

        // Assert
        Assert.That(result, Is.True);
    }

    private AdaptivityRule CreateSampleRule()
    {
        var trigger = Substitute.For<IAdaptivityTrigger>();
        var action = Substitute.For<IAdaptivityAction>();
        return new AdaptivityRule(trigger, action);
    }
}