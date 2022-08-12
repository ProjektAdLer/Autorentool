using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.GradeHistory.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.GradeHistory.xml;

[TestFixture]
public class ActivitiesGradeHistoryXmlUt
{
    [Test]
    public void ActivitiesGradeHistoryXmlGradeHistory_DefaultConstructor_AllParametersSet()
    {
        //Arrange

        //Act
        var systemUnderTest = new ActivitiesGradeHistoryXmlGradeHistory();
        
        //Assert
        Assert.That(systemUnderTest.GradeGrades, Is.EqualTo(""));
        
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

        //Act 
        systemUnderTest.Serialize("h5pactivity", "1");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1", "grade_history.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
    
}