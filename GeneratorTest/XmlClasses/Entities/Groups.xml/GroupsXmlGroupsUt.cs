using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Groups.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Groups.xml;

[TestFixture]
public class GroupsXmlUt
{

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
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Combine(curWorkDir, "XMLFilesForExport"));
        
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
    