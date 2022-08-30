using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.H5PActivity.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.H5pActivity.xml;

[TestFixture]
public class ActivitiesH5PActivityXmlActivityUt
{

    [Test]
    public void ActivitiesH5PActivityXmlActivity_StandardConstructor_AllParametersSet()
    {
        //Arrange
        // ReSharper disable once InconsistentNaming
        var h5pActivity = new ActivitiesH5PActivityXmlH5PActivity();
        
        var systemUnderTest = new ActivitiesH5PActivityXmlActivity();
        
        //Act
        systemUnderTest.H5pactivity = h5pActivity;
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.H5pactivity, Is.EqualTo(h5pActivity));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleId, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleName, Is.EqualTo(""));
            Assert.That(systemUnderTest.ContextId, Is.EqualTo(""));
        });
    }

    [Test]
    public void ActivitiesH5PActivityXmlActivity_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000"));
        
        var h5pActivity = new ActivitiesH5PActivityXmlH5PActivity();
        var systemUnderTest = new ActivitiesH5PActivityXmlActivity();
        systemUnderTest.H5pactivity = h5pActivity;
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize("h5pactivity", "1000");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000", "h5pactivity.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
        
    }
}