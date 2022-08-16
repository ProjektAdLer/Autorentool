using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Grades.xml;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Grades.xml;

[TestFixture]
public class ActivitiesGradesXmlActivityGradebookUt
{
    
    [Test]
    public void ActivitiesGradesXmlActivityGradebook_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var gradeitem = Substitute.For<ActivitiesGradesXmlGradeItem>();
        var gradeitems = Substitute.For<ActivitiesGradesXmlGradeItems>();
        gradeitems.GradeItem = gradeitem;
        var systemUnderTest = new ActivitiesGradesXmlActivityGradebook();

        //Act
        systemUnderTest.GradeItems = gradeitems;

        //Assert
        Assert.That(systemUnderTest.GradeItems, Is.EqualTo(gradeitems));
        Assert.That(systemUnderTest.GradeLetters, Is.EqualTo(""));
    }

    [Test]
    public void ActivitiesGradesXmlActivityGradebook_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2"));
        
        var gradeitem = new ActivitiesGradesXmlGradeItem();
        var gradeitems = new ActivitiesGradesXmlGradeItems();
        gradeitems.GradeItem = gradeitem;
        
        var gradeActivityGradebook = new ActivitiesGradesXmlActivityGradebook();
        gradeActivityGradebook.GradeItems = gradeitems;
        
        //Act 
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        gradeActivityGradebook.Serialize("h5pactivity", "2");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2", "grades.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
    
}