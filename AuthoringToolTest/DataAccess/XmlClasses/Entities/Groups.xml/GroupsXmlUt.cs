using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.Groups.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Groups.xml;

[TestFixture]
public class GroupsXmlUt
{
    
    [Test]
    public void GroupsXmlGroupingsList_StandardConstructor_AllParametersSet()
    {
        //Arrange
       
        //Act
        var systemUnderTest = new GroupsXmlGroupingsList();
        
        //Assert
        Assert.That(systemUnderTest.Groupings, Is.EqualTo(""));

    }

    [Test]
    public void GroupsXmlGroups_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var groupings = new GroupsXmlGroupingsList();
        
        //Act
        var systemUnderTest = new GroupsXmlGroups();
        systemUnderTest.GroupingsList = groupings;
        
        //Assert
        Assert.That(systemUnderTest.GroupingsList, Is.EqualTo(groupings));
    }
    

    
    [Test]
    public void GroupsXmlGroupingsList_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var groupingsList = new GroupsXmlGroupingsList();
        var systemUnderTest = new GroupsXmlGroups();
        systemUnderTest.GroupingsList = (groupingsList);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "groups.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}
    