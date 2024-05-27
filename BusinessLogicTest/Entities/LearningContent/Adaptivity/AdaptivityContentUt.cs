using BusinessLogic.Entities.LearningContent.Adaptivity;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.LearningContent.Adaptivity;

[TestFixture]
public class AdaptivityContentUt
{
    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var content = CreateSampleContent();

        // Act
        var result1 = content.Equals(null);
        var result2 = content!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var content = CreateSampleContent();

        // Act
        var result = content.Equals((object)content);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var content = CreateSampleContent();
        var other = new object();

        // Act
        var result = content.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsAdaptivityContentWithSameValues_ReturnsFalse()
    {
        // Arrange
        var content1 = CreateSampleContent();
        var content2 = CreateSampleContent();

        // Act
        var result = content1.Equals((object?)content2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsAdaptivityContentWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var content1 = CreateSampleContent();
        var content2 = new AdaptivityContent(
            new List<IAdaptivityTask> { Substitute.For<IAdaptivityTask>() })
        {
            Name = "Different Content"
        };

        // Act
        var result = content1.Equals((object?)content2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var content = CreateSampleContent();
        var expectedHashCode = HashCode.Combine(content.Tasks, content.Name);

        // Act
        var result = content.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsFalse()
    {
        // Arrange
        var content1 = CreateSampleContent();
        var content2 = CreateSampleContent();

        // Act
        var result = content1 == content2;

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var content1 = CreateSampleContent();
        var content2 = new AdaptivityContent(
            new List<IAdaptivityTask> { Substitute.For<IAdaptivityTask>() })
        {
            Name = "Different Content"
        };

        // Act
        var result = content1 != content2;

        // Assert
        Assert.That(result, Is.True);
    }

    private AdaptivityContent CreateSampleContent()
    {
        return new AdaptivityContent(
            new List<IAdaptivityTask> { Substitute.For<IAdaptivityTask>() })
        {
            Name = "Sample Content"
        };
    }
}