using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity.Action;

[TestFixture]
public class ElementReferenceActionUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var action = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment");

        // Act
        var result1 = action.Equals(null);
        var result2 = action!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var action = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment");

        // Act
        var result1 = action.Equals(action);
        var result2 = action.Equals((object)action);

        // Assert
        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var action = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment");
        var other = new object();

        // Act
        var result = action.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsElementReferenceActionWithSameValues_ReturnsFalse()
    {
        // Arrange
        var elementId = Guid.NewGuid();
        var action1 = new ElementReferenceAction(elementId, "Sample Comment");
        var action2 = new ElementReferenceAction(elementId, "Sample Comment");

        // Act
        var result = action1.Equals((object?)action2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsElementReferenceActionWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var action1 = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment 1");
        var action2 = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment 2");

        // Act
        var result = action1.Equals((object?)action2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var action = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment");
        var expectedHashCode = HashCode.Combine(action.ElementId, action.Id, action.Comment);

        // Act
        var result = action.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsFalse()
    {
        // Arrange
        var elementId = Guid.NewGuid();
        var action1 = new ElementReferenceAction(elementId, "Sample Comment");
        var action2 = new ElementReferenceAction(elementId, "Sample Comment");

        // Act
        var result = action1 == action2;

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var action1 = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment 1");
        var action2 = new ElementReferenceAction(Guid.NewGuid(), "Sample Comment 2");

        // Act
        var result = action1 != action2;

        // Assert
        Assert.That(result, Is.True);
    }
}