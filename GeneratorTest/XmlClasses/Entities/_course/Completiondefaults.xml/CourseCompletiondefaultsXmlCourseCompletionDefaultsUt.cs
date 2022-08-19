using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._course.Completiondefault.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Course.Completiondefaults.xml;

[TestFixture]
public class CourseCompletiondefaultsXmlCourseCompletionDefaults
{
    [Test]
    public void CourseCompletiondefaultsXmlCourseCompletionDefaults_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var courseCompletiondefault = new CourseCompletiondefaultXmlCourseCompletionDefaults();
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        courseCompletiondefault.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "completiondefaults.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}