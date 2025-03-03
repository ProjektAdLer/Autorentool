using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity.Action;

[TestFixture]
public class ContentReferenceActionUt
{
    [Test]
    public void Constructor_ThrowsArgumentException_WhenContentIsIAdaptivityContent()
    {
        // Arrange
        var content = Substitute.For<IAdaptivityContent>();

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentException>(() => new ContentReferenceAction(content, "Sample Comment"));
    }

    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var action = CreateSampleAction();

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
        var action = CreateSampleAction();

        // Act
        var result = action.Equals((object)action);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var action = CreateSampleAction();
        var other = new object();

        // Act
        var result = action.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsContentReferenceActionWithSameValues_ReturnsFalse()
    {
        // Arrange
        var content = Substitute.For<ILearningContent>();
        var action1 = new ContentReferenceAction(content, "Sample Comment");
        var action2 = new ContentReferenceAction(content, "Sample Comment");

        // Act
        var result = action1.Equals((object?)action2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsContentReferenceActionWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var content1 = Substitute.For<ILearningContent>();
        var content2 = Substitute.For<ILearningContent>();
        var action1 = new ContentReferenceAction(content1, "Sample Comment 1");
        var action2 = new ContentReferenceAction(content2, "Sample Comment 2");

        // Act
        var result = action1.Equals((object?)action2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var action = CreateSampleAction();
        var expectedHashCode = HashCode.Combine(action.Content, action.Id, action.Comment);

        // Act
        var result = action.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsFalse()
    {
        // Arrange
        var content = Substitute.For<ILearningContent>();
        var action1 = new ContentReferenceAction(content, "Sample Comment");
        var action2 = new ContentReferenceAction(content, "Sample Comment");

        // Act
        var result = action1 == action2;

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var content1 = Substitute.For<ILearningContent>();
        var content2 = Substitute.For<ILearningContent>();
        var action1 = new ContentReferenceAction(content1, "Sample Comment 1");
        var action2 = new ContentReferenceAction(content2, "Sample Comment 2");

        // Act
        var result = action1 != action2;

        // Assert
        Assert.That(result, Is.True);
    }

    private ContentReferenceAction CreateSampleAction()
    {
        var content = Substitute.For<ILearningContent>();
        return new ContentReferenceAction(content, "Sample Comment");
    }
}