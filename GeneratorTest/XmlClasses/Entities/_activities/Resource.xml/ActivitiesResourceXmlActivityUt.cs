using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.Resource.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Resource.xml;

[TestFixture]
public class ActivitiesResourceXmlActivityUt
{

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