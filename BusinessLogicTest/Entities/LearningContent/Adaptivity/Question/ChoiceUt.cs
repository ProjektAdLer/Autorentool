using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity.Question;

[TestFixture]
public class ChoiceUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var choice = new Choice("Sample Choice");

        // Act
        var result1 = choice.Equals(null);
        var result2 = choice!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var choice = new Choice("Sample Choice");

        // Act
        var result1 = choice.Equals(choice);
        var result2 = choice.Equals((object)choice);

        // Assert
        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var choice = new Choice("Sample Choice");
        var other = new object();

        // Act
        var result = choice.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsChoiceWithSameValues_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var choice1 = new Choice("Sample Choice") { Id = id };
        var choice2 = new Choice("Sample Choice") { Id = id };

        // Act
        var result = choice1.Equals((object?)choice2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsChoiceWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var choice1 = new Choice("Sample Choice 1");
        var choice2 = new Choice("Sample Choice 2");

        // Act
        var result = choice1.Equals((object?)choice2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var choice = new Choice("Sample Choice");
        var expectedHashCode = HashCode.Combine(choice.Text, choice.Id);

        // Act
        var result = choice.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var choice1 = new Choice("Sample Choice") { Id = id };
        var choice2 = new Choice("Sample Choice") { Id = id };

        // Act
        var result = choice1 == choice2;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var choice1 = new Choice("Sample Choice 1");
        var choice2 = new Choice("Sample Choice 2");

        // Act
        var result = choice1 != choice2;

        // Assert
        Assert.That(result, Is.True);
    }
}