using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities._activities.H5pActivity.xml;

[TestFixture]
public class ActivitiesH5PActivityXmlUt
{
    [Test]
    public void ActivitiesH5pActivityXml_H5pActivity_StandardConstructor_AllParametersSet()
    {
        //Arrange
        // ReSharper disable once InconsistentNaming
        
        //Act
        var systemUnderTest = new ActivitiesH5PActivityXmlH5PActivity();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Intro, Is.EqualTo(""));
            Assert.That(systemUnderTest.Introformat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Grade, Is.EqualTo("100"));
            Assert.That(systemUnderTest.Displayoptions, Is.EqualTo("15"));
            Assert.That(systemUnderTest.Enabletracking, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Grademethod, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Reviewmode, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Attempts, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
    }

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