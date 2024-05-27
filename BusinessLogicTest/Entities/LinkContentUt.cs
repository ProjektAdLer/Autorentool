using BusinessLogic.Entities.LearningContent.LinkContent;
using NUnit.Framework;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LinkContentUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var name = "foobar";
        var link = "https://www.google.com";

        var systemUnderTest = new LinkContent(name, link);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Link, Is.EqualTo(link));
        });
    }

    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var linkContent = new LinkContent("Name", "https://example.com");

        // Act
        var result1 = linkContent.Equals(null);
        var result2 = linkContent!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var linkContent = new LinkContent("Name", "https://example.com");

        // Act
        var result1 = linkContent.Equals(linkContent);
        var result2 = linkContent.Equals((object)linkContent);

        // Assert
        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var linkContent = new LinkContent("Name", "https://example.com");
        var other = new object();

        // Act
        var result = linkContent.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsLinkContentWithSameValues_ReturnsTrue()
    {
        // Arrange
        var linkContent1 = new LinkContent("Name", "https://example.com");
        var linkContent2 = new LinkContent("Name", "https://example.com");

        // Act
        var result = linkContent1.Equals((object?)linkContent2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsLinkContentWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var linkContent1 = new LinkContent("Name1", "https://example1.com");
        var linkContent2 = new LinkContent("Name2", "https://example2.com");

        // Act
        var result = linkContent1.Equals((object?)linkContent2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var linkContent = new LinkContent("Name", "https://example.com");
        var expectedHashCode = HashCode.Combine(linkContent.Name, linkContent.Link);

        // Act
        var result = linkContent.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsTrue()
    {
        // Arrange
        var linkContent1 = new LinkContent("Name", "https://example.com");
        var linkContent2 = new LinkContent("Name", "https://example.com");

        // Act
        var result = linkContent1 == linkContent2;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var linkContent1 = new LinkContent("Name1", "https://example1.com");
        var linkContent2 = new LinkContent("Name2", "https://example2.com");

        // Act
        var result = linkContent1 != linkContent2;

        // Assert
        Assert.That(result, Is.True);
    }
}