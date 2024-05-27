using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using NSubstitute;
using NUnit.Framework;
using Shared.Adaptivity;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity.Question;

[TestFixture]
public class MultipleChoiceMultipleResponseQuestionUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var question = CreateSampleQuestion();

        // Act
        var result1 = question.Equals(null);
        var result2 = question!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var question = CreateSampleQuestion();

        // Act
        var result1 = question.Equals((object)question);

        // Assert
        Assert.That(result1, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var question = CreateSampleQuestion();
        var other = new object();

        // Act
        var result = question.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsMultipleChoiceMultipleResponseQuestionWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var question1 = CreateSampleQuestion();
        var question2 = new MultipleChoiceMultipleResponseQuestion(
            20,
            new List<Choice> { new Choice("Choice A") },
            new List<Choice> { new Choice("Choice A") },
            new List<IAdaptivityRule> { Substitute.For<IAdaptivityRule>() },
            "Different Text",
            QuestionDifficulty.Hard
        );

        // Act
        var result = question1.Equals((object?)question2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var question = CreateSampleQuestion();
        var expectedHashCode = HashCode.Combine(
            question.Id,
            question.ExpectedCompletionTime,
            (int)question.Difficulty,
            question.Rules,
            question.Choices,
            question.CorrectChoices,
            question.Text
        );

        // Act
        var result = question.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    private MultipleChoiceMultipleResponseQuestion CreateSampleQuestion()
    {
        return new MultipleChoiceMultipleResponseQuestion(
            10,
            new List<Choice> { new Choice("Choice A"), new Choice("Choice B") },
            new List<Choice> { new Choice("Choice A") },
            new List<IAdaptivityRule> { Substitute.For<IAdaptivityRule>() },
            "Sample Text",
            QuestionDifficulty.Medium
        );
    }
}