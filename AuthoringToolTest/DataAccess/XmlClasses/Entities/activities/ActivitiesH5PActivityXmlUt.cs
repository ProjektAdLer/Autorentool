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
        var h5pactivity = new ActivitiesH5PActivityXmlH5PActivity();
        
        //Act
        h5pactivity.SetParameterts("h5pElementName",
            "currentTime", "currentTime", "", "1", "100",
            "15", "1", "1", "1", "", "h5pElementId");
        
        //Assert
        Assert.That(h5pactivity.Name, Is.EqualTo("h5pElementName"));
        Assert.That(h5pactivity.Timecreated, Is.EqualTo("currentTime"));
        Assert.That(h5pactivity.Timemodified, Is.EqualTo("currentTime"));
        Assert.That(h5pactivity.Intro, Is.EqualTo(""));
        Assert.That(h5pactivity.Introformat, Is.EqualTo("1"));
        Assert.That(h5pactivity.Grade, Is.EqualTo("100"));
        Assert.That(h5pactivity.Displayoptions, Is.EqualTo("15"));
        Assert.That(h5pactivity.Enabletracking, Is.EqualTo("1"));
        Assert.That(h5pactivity.Grademethod, Is.EqualTo("1"));
        Assert.That(h5pactivity.Reviewmode, Is.EqualTo("1"));
        Assert.That(h5pactivity.Attempts, Is.EqualTo(""));
        Assert.That(h5pactivity.Id, Is.EqualTo("h5pElementId"));
    }

    [Test]
    public void ActivitiesH5PActivityXmlActivity_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var h5pactivity = new ActivitiesH5PActivityXmlH5PActivity();
        h5pactivity.SetParameterts("h5pElementName",
            "currentTime", "currentTime", "", "1", "100",
            "15", "1", "1", "1", "", "h5pElementId");
        var activity = new ActivitiesH5PActivityXmlActivity();
        //Act
        activity.SetParameterts(h5pactivity, "h5pElementId", "h5pElementId", "h5pactivity", "h5pElementId");
        
        //Assert
        Assert.That(activity.H5pactivity, Is.EqualTo(h5pactivity));
        Assert.That(activity.Id, Is.EqualTo("h5pElementId"));
        Assert.That(activity.Moduleid, Is.EqualTo("h5pElementId"));
        Assert.That(activity.Modulename, Is.EqualTo("h5pactivity"));
        Assert.That(activity.Contextid, Is.EqualTo("h5pElementId"));
    }

    [Test]
    public void ActivitiesH5PActivityXmlActivity_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var readDsl = new ReadDSL();
        var h5pfactory = new XmlH5PFactory(readDsl, mockFileSystem, null, null, null, null,
            null, null, null, null, null, null, null, null, null,
            null, null, null, null);
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var h5pactivity = new ActivitiesH5PActivityXmlH5PActivity();
        h5pactivity.SetParameterts("h5pElementName",
            "currentTime", "currentTime", "", "1", "100",
            "15", "1", "1", "1", "", "h5pElementId");
        var activity = new ActivitiesH5PActivityXmlActivity();
        activity.SetParameterts(h5pactivity, "h5pElementId", "h5pElementId", "h5pactivity", "h5pElementId");
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        h5pfactory.CreateActivityFolder("1000");
        activity.Serialize("1000");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1000", "h5pactivity.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
        
    }
}