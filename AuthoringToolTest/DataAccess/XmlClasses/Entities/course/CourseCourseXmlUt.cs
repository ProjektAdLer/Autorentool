using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.course;
using AuthoringTool.DataAccess.XmlClasses.sections;
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
        courseCourse.SetParameters(courseCategory);
        
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
        courseCourse.SetParameters(courseCategory);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        courseCourse.Serialize();
        
        //Assert
        var pathXml = Path.Join(curWorkDir, "XMLFilesForExport");
        var pathXmlPartOne = Path.Join(pathXml, "course");
        var pathXmlFile = Path.Join(pathXmlPartOne, "course.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}