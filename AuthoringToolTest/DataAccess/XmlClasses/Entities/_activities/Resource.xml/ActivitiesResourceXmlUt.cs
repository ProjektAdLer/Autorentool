using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities._activities.Resource.xml;

[TestFixture]
public class ActivitiesResourceXmlUt
{
    [Test]
    public void ActivitiesResourceXmlResource_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new ActivitiesResourceXmlResource();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Intro, Is.EqualTo(""));
            Assert.That(systemUnderTest.IntroFormat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.TobeMigrated, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Legacyfiles, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Legacyfileslast, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Display, Is.EqualTo("0"));
            Assert.That(systemUnderTest.DisplayOptions, Is.EqualTo(""));
            Assert.That(systemUnderTest.FilterFiles, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Revision, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            
        });
    }

    [Test]
    public void ActivitiesResourceXmlActivity_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var resource = new ActivitiesResourceXmlResource();
        
        // Act
        var systemUnderTest = new ActivitiesResourceXmlActivity();
        systemUnderTest.Resource = resource;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Resource, Is.EqualTo(resource));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleId, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleName, Is.EqualTo(""));
            Assert.That(systemUnderTest.ContextId, Is.EqualTo(""));
        });
    }
    
    [Test]
    public void ActivitiesResourceXmlActivity_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        string activityName = "resource";
        string moduleId = "1";
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", activityName + "_" + moduleId));

        var resource = new ActivitiesResourceXmlResource();
        var systemUnderTest = new ActivitiesResourceXmlActivity();
        systemUnderTest.Resource = resource;

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize(activityName, moduleId);

        //Assert
        var pathXmlFile = Path.Join(currWorkDir, "XMLFilesForExport", "activities", 
            activityName + "_" + moduleId, "resource.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }

}