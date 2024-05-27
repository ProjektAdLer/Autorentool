using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using NSubstitute;
using NUnit.Framework;
using Shared.Adaptivity;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity;

[TestFixture]
public class AdaptivityTaskUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var task = CreateSampleTask();

        // Act
        var result1 = task.Equals(null);
        var result2 = task!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var task = CreateSampleTask();

        // Act
        var result = task.Equals((object)task);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var task = CreateSampleTask();
        var other = new object();

        // Act
        var result = task.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsAdaptivityTaskWithSameValues_ReturnsFalse()
    {
        // Arrange
        var task1 = CreateSampleTask();
        var task2 = CreateSampleTask();

        // Act
        var result = task1.Equals((object?)task2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsAdaptivityTaskWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var task1 = CreateSampleTask();
        var task2 = new AdaptivityTask(
            new List<IAdaptivityQuestion> { Substitute.For<IAdaptivityQuestion>() },
            QuestionDifficulty.Hard,
            "Different Task");

        // Act
        var result = task1.Equals((object?)task2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var task = CreateSampleTask();
        var expectedHashCode = HashCode.Combine(
            task.Questions,
            task.MinimumRequiredDifficulty.HasValue ? (int)task.MinimumRequiredDifficulty.Value : -1,
            task.Id);

        // Act
        var result = task.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsFalse()
    {
        // Arrange
        var task1 = CreateSampleTask();
        var task2 = CreateSampleTask();

        // Act
        var result = task1 == task2;

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var task1 = CreateSampleTask();
        var task2 = new AdaptivityTask(
            new List<IAdaptivityQuestion> { Substitute.For<IAdaptivityQuestion>() },
            QuestionDifficulty.Hard,
            "Different Task");

        // Act
        var result = task1 != task2;

        // Assert
        Assert.That(result, Is.True);
    }

    private AdaptivityTask CreateSampleTask()
    {
        return new AdaptivityTask(
            new List<IAdaptivityQuestion> { Substitute.For<IAdaptivityQuestion>() },
            QuestionDifficulty.Medium,
            "Sample Task");
    }
}