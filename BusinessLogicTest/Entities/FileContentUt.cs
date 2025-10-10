using BusinessLogic.Entities.LearningContent.FileContent;
using NUnit.Framework;
using Shared.H5P;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class FileContentUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var name = "foobar";
        var type = "barbaz";
        var content = "";

        var systemUnderTest = new FileContent(name, type, content);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Type, Is.EqualTo(type));
            Assert.That(systemUnderTest.Filepath, Is.EqualTo(content));
            Assert.That(systemUnderTest.IsH5P, Is.EqualTo(false));
            Assert.That(systemUnderTest.H5PState, Is.EqualTo(H5PContentState.NotValidated));
        });
    }

    [Test]
    public void Equals_OtherIsNull_ReturnsFalse()
    {
        // Arrange
        var fileContent = new FileContent("Name", "Type", "Filepath");

        // Act
        var result1 = fileContent.Equals(null);
        var result2 = fileContent!.Equals((object?)null);

        // Assert
        Assert.That(result1, Is.False);
        Assert.That(result2, Is.False);
    }

    [Test]
    public void Equals_OtherIsSameReference_ReturnsTrue()
    {
        // Arrange
        var fileContent = new FileContent("Name", "Type", "Filepath");

        // Act
        var result1 = fileContent.Equals(fileContent);
        var result2 = fileContent.Equals((object)fileContent);

        // Assert
        Assert.That(result1, Is.True);
        Assert.That(result2, Is.True);
    }

    [Test]
    public void Equals_OtherIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var fileContent = new FileContent("Name", "Type", "Filepath");
        var other = new object();

        // Act
        var result = fileContent.Equals(other);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_OtherIsFileContentWithSameValues_ReturnsTrue()
    {
        // Arrange
        var fileContent1 = new FileContent("Name", "Type", "Filepath");
        var fileContent2 = new FileContent("Name", "Type", "Filepath");

        // Act
        var result = fileContent1.Equals((object?)fileContent2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_OtherIsFileContentWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var fileContent1 = new FileContent("Name1", "Type1", "Filepath1");
        var fileContent2 = new FileContent("Name2", "Type2", "Filepath2");

        // Act
        var result = fileContent1.Equals((object?)fileContent2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var fileContent = new FileContent("Name", "Type", "Filepath");
        var expectedHashCode = HashCode.Combine(fileContent.Name, fileContent.Type, fileContent.Filepath);

        // Act
        var result = fileContent.GetHashCode();

        // Assert
        Assert.That(result, Is.EqualTo(expectedHashCode));
    }

    [Test]
    public void OperatorEqual_SameValues_ReturnsTrue()
    {
        // Arrange
        var fileContent1 = new FileContent("Name", "Type", "Filepath");
        var fileContent2 = new FileContent("Name", "Type", "Filepath");

        // Act
        var result = fileContent1 == fileContent2;

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void OperatorNotEqual_DifferentValues_ReturnsTrue()
    {
        // Arrange
        var fileContent1 = new FileContent("Name1", "Type1", "Filepath1");
        var fileContent2 = new FileContent("Name2", "Type2", "Filepath2");

        // Act
        var result = fileContent1 != fileContent2;

        // Assert
        Assert.That(result, Is.True);
    }
}