using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.activities;

[TestFixture]
public class ActivitiesModuleXmlUt
{
    [Test]
    public void ActivitiesModuleXmlModule_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var module = new ActivitiesModuleXmlModule();
        
        //Act
        module.SetParameterts("h5pactivity", "h5pElementId", "h5pElementId",
            "", "currentTime", "0", "0", "1",
            "1", "1", "0", "1",
            "1", "$@NULL@$", "0",
            "0", "$@NULL@$", "0", "", 
            "h5pElementId", "2021051700");
        
        //Assert
        Assert.That(module.Modulename, Is.EqualTo("h5pactivity"));
        Assert.That(module.Sectionid, Is.EqualTo("h5pElementId"));
        Assert.That(module.Sectionnumber, Is.EqualTo("h5pElementId"));
        Assert.That(module.Idnumber, Is.EqualTo(""));
        Assert.That(module.Added, Is.EqualTo("currentTime"));
        Assert.That(module.Score, Is.EqualTo("0"));
        Assert.That(module.Indent, Is.EqualTo("0"));
        Assert.That(module.Visible, Is.EqualTo("1"));
        Assert.That(module.Visibleoncoursepage, Is.EqualTo("1"));
        Assert.That(module.Visibleold, Is.EqualTo("1"));
        Assert.That(module.Groupmode, Is.EqualTo("0"));
        Assert.That(module.Groupingid, Is.EqualTo("1"));
        Assert.That(module.Completion, Is.EqualTo("1"));
        Assert.That(module.Completiongradeitemnumber, Is.EqualTo("$@NULL@$"));
        Assert.That(module.Completionview, Is.EqualTo("0"));
        Assert.That(module.Completionexpected, Is.EqualTo("0"));
        Assert.That(module.Availability, Is.EqualTo("$@NULL@$"));
        Assert.That(module.Showdescription, Is.EqualTo("0"));
        Assert.That(module.Tags, Is.EqualTo(""));
        Assert.That(module.Id, Is.EqualTo("h5pElementId"));
        Assert.That(module.Version, Is.EqualTo("2021051700"));
    }

    [Test]
    public void ActivitiesModuleXmlModule_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var readDsl = new ReadDSL();
        var h5pfactory = new XmlH5PFactory(readDsl, mockFileSystem, null, null, null, null,
            null, null, null, null, null, null, null, null, null,
            null, null, null, null);
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var module = new ActivitiesModuleXmlModule();
        module.SetParameterts("h5pactivity", "h5pElementId", "h5pElementId",
            "", "currentTime", "0", "0", "1",
            "1", "1", "0", "1",
            "1", "$@NULL@$", "0",
            "0", "$@NULL@$", "0", "", 
            "h5pElementId", "2021051700");
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        h5pfactory.CreateActivityFolder("2");
        module.Serialize("2");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2", "module.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}