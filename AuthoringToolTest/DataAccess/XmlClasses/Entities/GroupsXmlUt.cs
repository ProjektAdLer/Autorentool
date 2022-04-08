using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class GroupsXmlUt
{

    [Test]
    public void GroupsXmlGroups_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var groupingsList = new GroupsXmlGroupingsList();
        groupingsList.SetParameters("");

        var groups = new GroupsXmlGroups();
        
        //Act
        groups.SetParameters(groupingsList);
        
        //Assert
        Assert.That(groups.GroupingsList, Is.EqualTo(groupingsList));
    }
    
    [Test]
    public void GroupsXmlGroupingsList_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var groupingsList = new GroupsXmlGroupingsList();
        
        //Act
        groupingsList.SetParameters("");
        
        //Assert
        Assert.That(groupingsList.Groupings, Is.EqualTo(""));

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
        groupingsList.SetParameters("");
        var groups = new GroupsXmlGroups();
        groups.SetParameters(groupingsList);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        groups.Serialize();
        
        //Assert
        var pathXml = Path.Join(curWorkDir, "XMLFilesForExport");
        var pathXmlFile = Path.Join(pathXml, "groups.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}
    


