using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.course;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.course;

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