using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.course;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.course;

[TestFixture]
public class CourseCourseXmlUt
{
    [Test]
    public void CourseCourseXmlCategory_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var courseCategory = new CourseCourseXmlCategory();
        
        //Act
        courseCategory.SetParameters("Miscellaneous", "$@NULL@$", "1"); 
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(courseCategory.Name, Is.EqualTo("Miscellaneous"));
            Assert.That(courseCategory.Description, Is.EqualTo("$@NULL@$"));
            Assert.That(courseCategory.Id, Is.EqualTo("1"));
        });
    }
    
    [Test]
    public void CourseCourseXmlCourse_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var courseCategory = new CourseCourseXmlCategory();
        courseCategory.SetParameters("Miscellaneous", "$@NULL@$", "1"); 
        var courseCourse = new CourseCourseXmlCourse();
        
        //Act
        courseCourse.SetParameters("learningWorld.identifier.value","learningWorld.identifier.value",
            "","","1","topics","1","5","1645484400",
            "2221567452","0","0","0","0","1",
            "0","0","0","","","currentTime","currentTime",
            "0","1","1","1","0",
            "0","0",courseCategory, "", "", "1", "1");
        
        //Assert
        Assert.That(courseCourse.Category, Is.EqualTo(courseCategory));
    }
    
    [Test]

    public void CourseCourseXmlCourse_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var courseCategory = new CourseCourseXmlCategory();
        courseCategory.SetParameters("Miscellaneous", "$@NULL@$", "1"); 
        var courseCourse = new CourseCourseXmlCourse();
        courseCourse.SetParameters("learningWorld.identifier.value","learningWorld.identifier.value",
            "","","1","topics","1","5","1645484400",
            "2221567452","0","0","0","0","1",
            "0","0","0","","","currentTime","currentTime",
            "0","1","1","1","0",
            "0","0",courseCategory, "", ""
            , "1", "1");

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        courseCourse.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "course.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}