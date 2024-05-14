using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._course.Enrolments.xml;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities._course.Enrolments.xml;

[TestFixture]
public class CourseEnrolmentsXmlEnrolmentsUt
{
    
    [Test]
    public void CourseEnrolmentsXmlEnrolments_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrols = new CourseEnrolmentsXmlEnrols();
        
        enrolmentsEnrols.Enrol.Add(enrolmentsEnrol1);
        enrolmentsEnrols.Enrol.Add(enrolmentsEnrol2);
        enrolmentsEnrols.Enrol.Add(enrolmentsEnrol3);
        
        var systemUnderTest = new CourseEnrolmentsXmlEnrolments();

        //Act
        systemUnderTest.Enrols = enrolmentsEnrols;
        
        //Assert
        Assert.That(systemUnderTest.Enrols, Is.EqualTo(enrolmentsEnrols));
    }
    
    [Test]

    public void CourseEnrolmentsXmlEnrolments_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Combine(curWorkDir, "XMLFilesForExport","course"));
        
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();

        var enrolmentsEnrols = new CourseEnrolmentsXmlEnrols();

        enrolmentsEnrols.Enrol.Add(enrolmentsEnrol1);
        enrolmentsEnrols.Enrol.Add(enrolmentsEnrol2);
        enrolmentsEnrols.Enrol.Add(enrolmentsEnrol3);
        
        var systemUnderTest = new CourseEnrolmentsXmlEnrolments();
        systemUnderTest.Enrols = enrolmentsEnrols;

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "enrolments.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}