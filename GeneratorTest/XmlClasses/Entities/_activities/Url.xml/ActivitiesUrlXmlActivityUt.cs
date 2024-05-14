using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.Url.xml;
using NSubstitute;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities._activities.Url.xml;

[TestFixture]
public class ActivitiesUrlXmlActivityUt
{
    [Test]
    public void ActivitiesUrlXmlActivity_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var mockUrl = Substitute.For<ActivitiesUrlXmlUrl>();
        
        // Act
        var systemUnderTest = new ActivitiesUrlXmlActivity();
        systemUnderTest.Url = mockUrl;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Url, Is.EqualTo(mockUrl));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.Moduleid, Is.EqualTo(""));
            Assert.That(systemUnderTest.Modulename, Is.EqualTo("url"));
            Assert.That(systemUnderTest.Contextid, Is.EqualTo(""));
            
        });
    }

    [Test]
    public void ActivitiesUrlXmlActivity_Serialize_XmlFileWritten()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", "url_2"));

        var systemUnderTest = new ActivitiesUrlXmlActivity();
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        
        // Act
        systemUnderTest.Serialize("url", "2");
        
        // Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "url_2", "url.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
    
}