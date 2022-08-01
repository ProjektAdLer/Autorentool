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
    public void CourseEnrolmentsXmlEnrol_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new CourseEnrolmentsXmlEnrol();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.EnrolMethod, Is.EqualTo(""));
            Assert.That(systemUnderTest.Status, Is.EqualTo(""));
            Assert.That(systemUnderTest.Name, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.EnrolPeriod, Is.EqualTo("0"));
            Assert.That(systemUnderTest.EnrolStartDate, Is.EqualTo("0"));
            Assert.That(systemUnderTest.EnrolEndDate, Is.EqualTo("0"));
            Assert.That(systemUnderTest.ExpiryNotify, Is.EqualTo("0"));
            Assert.That(systemUnderTest.ExpiryThreshold, Is.EqualTo("86400"));
            Assert.That(systemUnderTest.NotifyAll, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Password, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Cost, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Currency, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.RoleId, Is.EqualTo(""));
            Assert.That(systemUnderTest.CustomInt1, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt2, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt3, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt4, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt5, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt6, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt7, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt8, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomChar1, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomChar2, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomChar3, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText1, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText2, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText3, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText4, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.User_enrolments, Is.EqualTo(""));
            
        });
    }
    
    [Test]
    public void CourseEnrolmentsXmlEnrols_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();
        var systemUnderTest = new CourseEnrolmentsXmlEnrols();
        
        //Act
        systemUnderTest.Enrol.Add(enrolmentsEnrol1);
        systemUnderTest.Enrol.Add(enrolmentsEnrol2);
        systemUnderTest.Enrol.Add(enrolmentsEnrol3);

        var expected_result = new List<CourseEnrolmentsXmlEnrol?>();
        expected_result.Add(enrolmentsEnrol1);
        expected_result.Add(enrolmentsEnrol2);
        expected_result.Add(enrolmentsEnrol3);
        
        //Assert
        Assert.That(systemUnderTest.Enrol, Is.EqualTo(expected_result));
    }
    
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
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
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