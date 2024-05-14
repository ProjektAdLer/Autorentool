using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities._activities.GradeHistory.xml;

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
        
        var curWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Combine(curWorkDir, "XMLFilesForExport","activities", "h5pactivity_1"));
        var systemUnderTest = new ActivitiesGradeHistoryXmlGradeHistory();
        
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;

        //Act 
        systemUnderTest.Serialize("h5pactivity", "1");
        
        //Assert
        var path = Path.Join(curWorkDir, "XMLFilesForExport","activities", "h5pactivity_1", "grade_history.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
    
}