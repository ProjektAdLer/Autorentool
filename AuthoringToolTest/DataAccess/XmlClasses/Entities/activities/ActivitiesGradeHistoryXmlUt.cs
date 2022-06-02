using System.IO;
using System.IO.Abstractions;
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
        var gradehistory = new ActivitiesGradeHistoryXmlGradeHistory();
        
        //Act
        gradehistory.SetParameterts("");
        
        //Assert
        Assert.That(gradehistory.Grade_grades, Is.EqualTo(""));
        
    }

    [Test]
    public void ActivitiesGradeHistoryXmlGradeHistory_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var readDsl = new ReadDSL();
        var h5pfactory = new XmlH5PFactory(readDsl, mockFileSystem, null, null, null, null,
            null, null, null, null, null, null, null, null, null,
            null, null, null, null);
        
        var gradehistory = new ActivitiesGradeHistoryXmlGradeHistory();
        gradehistory.SetParameterts("");
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        //Act 
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        h5pfactory.CreateActivityFolder("1");
        gradehistory.Serialize("1");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_1", "grade_history.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
    
}