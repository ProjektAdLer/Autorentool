using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.course;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.course;

[TestFixture]
public class CourseEnrolmentsXmlUt
{
    [Test]
    public void CourseEnrolmentsXmlEnrol_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        
        //Act
        enrolmentsEnrol1.SetParametersShort("5", "153", "manual", "0");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(enrolmentsEnrol1.Roleid, Is.EqualTo("5"));
            Assert.That(enrolmentsEnrol1.Id, Is.EqualTo("153"));
            Assert.That(enrolmentsEnrol1.Enrolchild, Is.EqualTo("manual"));
            Assert.That(enrolmentsEnrol1.Status, Is.EqualTo("0"));
        });
    }
    
    [Test]
    public void CourseEnrolmentsXmlEnrols_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol1.SetParametersShort("5", "153", "manual", "0");
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol2.SetParametersShort("0", "154", "guest", "1");
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol3.SetParametersFull("5", "155", "self", "1", "0", "0", "0", "1", "0", "1");
        var enrolmentsEnrols = new CourseEnrolmentsXmlEnrols();
        
        //Act
        enrolmentsEnrols.SetParameters(enrolmentsEnrol1, enrolmentsEnrol2, enrolmentsEnrol3);

        var expected_result = new List<CourseEnrolmentsXmlEnrol?>();
        expected_result.Add(enrolmentsEnrol1);
        expected_result.Add(enrolmentsEnrol2);
        expected_result.Add(enrolmentsEnrol3);
        
        //Assert
        Assert.That(enrolmentsEnrols.Enrol, Is.EqualTo(expected_result));
    }
    
    [Test]
    public void CourseEnrolmentsXmlEnrolments_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol1.SetParametersShort("5", "153", "manual", "0");
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol2.SetParametersShort("0", "154", "guest", "1");
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol3.SetParametersFull("5", "155", "self", "1", "0", "0", "0", "1", "0", "1");
        var enrolmentsEnrols = new CourseEnrolmentsXmlEnrols();
        enrolmentsEnrols.SetParameters(enrolmentsEnrol1, enrolmentsEnrol2, enrolmentsEnrol3);
        var enrolmentsEnrolments = new CourseEnrolmentsXmlEnrolments();

        //Act
        enrolmentsEnrolments.SetParameters(enrolmentsEnrols);
        
        //Assert
        Assert.That(enrolmentsEnrolments.Enrols, Is.EqualTo(enrolmentsEnrols));
    }
    
    [Test]

    public void CourseEnrolmentsXmlEnrolments_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol1.SetParametersShort("5", "153", "manual", "0");
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol2.SetParametersShort("0", "154", "guest", "1");
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();
        enrolmentsEnrol3.SetParametersFull("5", "155", "self", "1", "0", "0", "0", "1", "0", "1");
        var enrolmentsEnrols = new CourseEnrolmentsXmlEnrols();
        enrolmentsEnrols.SetParameters(enrolmentsEnrol1, enrolmentsEnrol2, enrolmentsEnrol3);
        var enrolmentsEnrolments = new CourseEnrolmentsXmlEnrolments();
        enrolmentsEnrolments.SetParameters(enrolmentsEnrols);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        enrolmentsEnrolments.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "enrolments.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}