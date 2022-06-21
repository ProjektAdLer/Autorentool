using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.activities;

[TestFixture]
public class ActivitiesH5PActivityXmlUt
{
    [Test]
    public void ActivitiesH5pActivityXmlH5pActiviy_SetParameters_ObjectAreEqual()
    {
        //Arrange
        // ReSharper disable once InconsistentNaming
        var h5pActivity = new ActivitiesH5PActivityXmlH5PActivity();
        
        //Act
        h5pActivity.SetParameterts("h5pElementName",
            "currentTime", "currentTime", "", "1", "100",
            "15", "1", "1", "1", "", "h5pElementId");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(h5pActivity.Name, Is.EqualTo("h5pElementName"));
            Assert.That(h5pActivity.Timecreated, Is.EqualTo("currentTime"));
            Assert.That(h5pActivity.Timemodified, Is.EqualTo("currentTime"));
            Assert.That(h5pActivity.Intro, Is.EqualTo(""));
            Assert.That(h5pActivity.Introformat, Is.EqualTo("1"));
            Assert.That(h5pActivity.Grade, Is.EqualTo("100"));
            Assert.That(h5pActivity.Displayoptions, Is.EqualTo("15"));
            Assert.That(h5pActivity.Enabletracking, Is.EqualTo("1"));
            Assert.That(h5pActivity.Grademethod, Is.EqualTo("1"));
            Assert.That(h5pActivity.Reviewmode, Is.EqualTo("1"));
            Assert.That(h5pActivity.Attempts, Is.EqualTo(""));
            Assert.That(h5pActivity.Id, Is.EqualTo("h5pElementId"));
        });
    }

    [Test]
    public void ActivitiesH5PActivityXmlActivity_SetParameters_ObjectAreEqual()
    {
        //Arrange
        // ReSharper disable once InconsistentNaming
        var h5pActivity = new ActivitiesH5PActivityXmlH5PActivity();
        
        var systemUnderTest = new ActivitiesH5PActivityXmlActivity();
        //Act
        systemUnderTest.SetParameterts(h5pActivity, "h5pElementId", "h5pElementId", "h5pactivity", "h5pElementId");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.H5pactivity, Is.EqualTo(h5pActivity));
            Assert.That(systemUnderTest.Id, Is.EqualTo("h5pElementId"));
            Assert.That(systemUnderTest.Moduleid, Is.EqualTo("h5pElementId"));
            Assert.That(systemUnderTest.Modulename, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.Contextid, Is.EqualTo("h5pElementId"));
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
        h5pActivity.SetParameterts("h5pElementName",
            "currentTime", "currentTime", "", "1", "100",
            "15", "1", "1", "1", "", "h5pElementId");
        var systemUnderTest = new ActivitiesH5PActivityXmlActivity();
        systemUnderTest.SetParameterts(h5pActivity, "h5pElementId", "h5pElementId", "h5pactivity", "h5pElementId");
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize("h5pactivity", "1000");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000", "h5pactivity.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
        
    }
}