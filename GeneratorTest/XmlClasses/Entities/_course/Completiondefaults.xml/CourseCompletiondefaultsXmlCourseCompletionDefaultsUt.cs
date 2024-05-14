using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._course.Completiondefault.xml;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities._course.Completiondefaults.xml;

[TestFixture]
public class CourseCompletiondefaultsXmlCourseCompletionDefaults
{
    [Test]
    public void CourseCompletiondefaultsXmlCourseCompletionDefaults_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Combine(curWorkDir, "XMLFilesForExport","course"));
        
        var courseCompletiondefault = new CourseCompletiondefaultXmlCourseCompletionDefaults();
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        courseCompletiondefault.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "completiondefaults.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}