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
        
        var groupingsList = new GroupsXmlGroupingsList();
        groupingsList.SetParameters("");
        var groups = new GroupsXmlGroups();
        groups.SetParameters(groupingsList);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        groups.Serialize();
        
        //Assert
        Assert.That(mockFileSystem.FileExists("C:\\XMLFilesForExport\\groups.xml"), Is.True);
    }
}
    


