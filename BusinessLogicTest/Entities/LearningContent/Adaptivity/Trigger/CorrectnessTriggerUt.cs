using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using NUnit.Framework;
using Shared.Adaptivity;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity.Trigger;

[TestFixture]
public class CorrectnessTriggerUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var correctnessTrigger = new CorrectnessTrigger(AnswerResult.Correct);

        // Act
        var result1 = correctnessTrigger.Equals(null);
        var result2 = correctnessTrigger!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var correctnessTrigger = new CorrectnessTrigger(AnswerResult.Correct);

        // Act
        var result1 = correctnessTrigger.Equals(correctnessTrigger);
        var result2 = correctnessTrigger.Equals((object)correctnessTrigger);

        // Assert
        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var correctnessTrigger = new CorrectnessTrigger(AnswerResult.Correct);
        var other = new object();

        // Act
        var result = correctnessTrigger.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsCorrectnessTriggerWithSameValues_ReturnsTrue()
    {
        // Arrange
        var correctnessTrigger1 = new CorrectnessTrigger(AnswerResult.Correct);
        var correctnessTrigger2 = new CorrectnessTrigger(AnswerResult.Correct);

        // Act
        var result = correctnessTrigger1.Equals((object?)correctnessTrigger2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsCorrectnessTriggerWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var correctnessTrigger1 = new CorrectnessTrigger(AnswerResult.Correct);
        var correctnessTrigger2 = new CorrectnessTrigger(AnswerResult.Incorrect);

        // Act
        var result = correctnessTrigger1.Equals((object?)correctnessTrigger2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var correctnessTrigger = new CorrectnessTrigger(AnswerResult.Correct);
        var expectedHashCode = (int)AnswerResult.Correct;

        // Act
        var result = correctnessTrigger.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsTrue()
    {
        // Arrange
        var correctnessTrigger1 = new CorrectnessTrigger(AnswerResult.Correct);
        var correctnessTrigger2 = new CorrectnessTrigger(AnswerResult.Correct);

        // Act
        var result = correctnessTrigger1 == correctnessTrigger2;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var correctnessTrigger1 = new CorrectnessTrigger(AnswerResult.Correct);
        var correctnessTrigger2 = new CorrectnessTrigger(AnswerResult.Incorrect);

        // Act
        var result = correctnessTrigger1 != correctnessTrigger2;

        // Assert
        Assert.That(result, Is.True);
    }
}