using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.activities;

[TestFixture]
public class ActivitiesGradeHistoryXmlUt
{
    [Test]
    public void ActivitiesGradeHistoryXmlGradeHistory_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var systemUnderTest = new ActivitiesGradeHistoryXmlGradeHistory();
        
        //Act
        systemUnderTest.SetParameterts("foo");
        
        //Assert
        Assert.That(systemUnderTest.Grade_grades, Is.EqualTo("foo"));
        
    }

    [Test]
    public void ActivitiesGradeHistoryXmlGradeHistory_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Combine(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1"));
        var systemUnderTest = new ActivitiesGradeHistoryXmlGradeHistory();
        
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.SetParameterts("");
        
        //Act 
        systemUnderTest.Serialize("h5pactivity", "1");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1", "grade_history.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
    
}