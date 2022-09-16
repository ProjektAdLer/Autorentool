using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.Label.xml;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Label.xml;

[TestFixture]
public class ActivitiesLabelXmlActivityUt
{
    [Test]
    public void ActivitiesLabelXmlActivity_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var mockLabel = Substitute.For<ActivitiesLabelXmlLabel>();

        // Act
        var systemUnderTest = new ActivitiesLabelXmlActivity();
        systemUnderTest.Label = mockLabel;

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(systemUnderTest.Label, Is.EqualTo(mockLabel));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleId, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleName, Is.EqualTo("label"));
            Assert.That(systemUnderTest.ContextId, Is.EqualTo(""));

        });
    }

    [Test]
    public void ActivitiesLabelXmlActivity_Serialize_XmlFileWritten()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", "label_1000"));
        

        var systemUnderTest = new ActivitiesLabelXmlActivity();
        

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize("label", "1000");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "label_1000", "label.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);

    }
}