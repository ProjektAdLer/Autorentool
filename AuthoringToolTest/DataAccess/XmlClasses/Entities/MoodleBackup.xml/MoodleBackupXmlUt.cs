using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class MoodleBackupXmlUt
{
    
    [Test]
    public void MoodleBackupXmlDetail_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        
        //Act
        var systemUnderTest = new MoodleBackupXmlDetail();
        
        //Assert
        Assert.That(systemUnderTest.Type, Is.EqualTo("course"));
        Assert.That(systemUnderTest.Format, Is.EqualTo("moodle2"));
        Assert.That(systemUnderTest.Interactive, Is.EqualTo("1"));
        Assert.That(systemUnderTest.Mode, Is.EqualTo("10"));
        Assert.That(systemUnderTest.Execution, Is.EqualTo("1"));
        Assert.That(systemUnderTest.ExecutionTime, Is.EqualTo("0"));
        Assert.That(systemUnderTest.BackupId, Is.EqualTo("36d63c7b4624cf6a79e0405be770974d"));
    }
    
    [Test]
    public void MoodleBackupXmlDetails_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        var systemUnderTest = new MoodleBackupXmlDetails();
        
        //Act
        systemUnderTest.Detail = moodlebackupDetail;

        //Assert
        Assert.That(systemUnderTest.Detail, Is.EqualTo(moodlebackupDetail));
    }
    
    [Test]
    public void MoodleBackupXmlSection_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new MoodleBackupXmlSection();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SectionId, Is.EqualTo(""));
            Assert.That(systemUnderTest.Title, Is.EqualTo(""));
            Assert.That(systemUnderTest.Directory, Is.EqualTo(""));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSections_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupSection = new MoodleBackupXmlSection();

        var systemUnderTest = new MoodleBackupXmlSections();

        //Act
        systemUnderTest.Section.Add(moodlebackupSection);
        
        //Assert
        Assert.That(systemUnderTest.Section, Is.EquivalentTo(new List<MoodleBackupXmlSection> { moodlebackupSection }));
    }
    
    
    [Test]
    public void MoodleBackupXmlCourse_StandardConstructor_AllParametersSet()
    {
        //Arrange

        //Act
        var systemUnderTest = new MoodleBackupXmlCourse();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CourseId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Title, Is.EqualTo(""));
            Assert.That(systemUnderTest.Directory, Is.EqualTo("course"));
        });
        
    }

    [Test]
    public void MoodleBackupXmlActivity_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new MoodleBackupXmlActivity();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ModuleId, Is.EqualTo(""));
            Assert.That(systemUnderTest.SectionId, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleName, Is.EqualTo(""));
            Assert.That(systemUnderTest.Title, Is.EqualTo(""));
            Assert.That(systemUnderTest.Directory, Is.EqualTo(""));
        });
        
    }

    [Test]
    public void MoodleBackupXmlActivities_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupActivity = new MoodleBackupXmlActivity();
        var systemUnderTest = new MoodleBackupXmlActivities();
        
        //Act
        systemUnderTest.Activity.Add(moodlebackupActivity);
        
        //Assert
        Assert.That(systemUnderTest.Activity, Is.EquivalentTo(new List<MoodleBackupXmlActivity> { moodlebackupActivity }));
    }
   
    
    
    [Test]
    public void MoodleBackupXmlContents_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupSections = new MoodleBackupXmlSections();
        var moodlebackupActivities = new MoodleBackupXmlActivities();
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        var systemUnderTest = new MoodleBackupXmlContents();
        
        //Act
        systemUnderTest.Sections = moodlebackupSections;
        systemUnderTest.Activities = moodlebackupActivities;
        systemUnderTest.Course = moodlebackupCourse;
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Sections, Is.EqualTo(moodlebackupSections));
            Assert.That(systemUnderTest.Activities, Is.EqualTo(moodlebackupActivities));
            Assert.That(systemUnderTest.Course, Is.EqualTo(moodlebackupCourse));
        });
    }
    
    
    [Test]
    public void MoodleBackupXmlSetting_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new MoodleBackupXmlSetting();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Value, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Level, Is.EqualTo("root"));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSetting_ConstructorSection_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new MoodleBackupXmlSetting("level", "name", "value", "Section", true);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo("name"));
            Assert.That(systemUnderTest.Value, Is.EqualTo("value"));
            Assert.That(systemUnderTest.Level, Is.EqualTo("level"));
            Assert.That(systemUnderTest.Section, Is.EqualTo("Section"));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSetting_ConstructorActivity_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new MoodleBackupXmlSetting("level", "name", "value", "Activity", false);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo("name"));
            Assert.That(systemUnderTest.Value, Is.EqualTo("value"));
            Assert.That(systemUnderTest.Level, Is.EqualTo("level"));
            Assert.That(systemUnderTest.Activity, Is.EqualTo("Activity"));
        });
        
    }

    [Test]
    public void MoodleBackupXmlSettings_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupSetting = new MoodleBackupXmlSetting();
        var systemUnderTest = new MoodleBackupXmlSettings();
        
        //Act
        systemUnderTest.Setting.Add(moodlebackupSetting);
        
        //Assert
        Assert.That(systemUnderTest.Setting, Is.EquivalentTo(new List<MoodleBackupXmlSetting> { moodlebackupSetting }));
    }
   
    
    [Test]
    public void MoodleBackupXmlInformation_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var systemUnderTest = new MoodleBackupXmlInformation();
        
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.Detail = (moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.Section.Add(moodlebackupSection);
        
        var moodlebackupCourse = new MoodleBackupXmlCourse();

        var moodlebackupContents = new MoodleBackupXmlContents();

        
        MoodleBackupXmlActivities activities = new MoodleBackupXmlActivities();
        activities.Activity.Add(new MoodleBackupXmlActivity());
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        moodlebackupSettings.Setting.Add(moodlebackupSetting1);
        
        //Act

        systemUnderTest.Details = moodlebackupDetails;
        systemUnderTest.Settings = moodlebackupSettings;
        systemUnderTest.Contents = moodlebackupContents;
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo("C#_AuthoringTool_Created_Backup.mbz"));
            Assert.That(systemUnderTest.MoodleVersion, Is.EqualTo("2021051703"));
            Assert.That(systemUnderTest.MoodleRelease, Is.EqualTo("3.11.3 (Build: 20210913)"));
            Assert.That(systemUnderTest.BackupVersion, Is.EqualTo("2021051700"));
            Assert.That(systemUnderTest.BackupRelease, Is.EqualTo("3.11"));
            Assert.That(systemUnderTest.BackupDate, Is.EqualTo(""));
            Assert.That(systemUnderTest.MnetRemoteUsers, Is.EqualTo("0"));
            Assert.That(systemUnderTest.IncludeFiles, Is.EqualTo("1"));
            Assert.That(systemUnderTest.IncludeFileReferencesToExternalContent, Is.EqualTo("0"));
            Assert.That(systemUnderTest.OriginalWwwRoot, Is.EqualTo("https://moodle.cluuub.xyz"));
            Assert.That(systemUnderTest.OriginalSiteIdentifierHash, Is.EqualTo("c9629ccd3c092478330b78bdf4dcdb18"));
            Assert.That(systemUnderTest.OriginalCourseId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.OriginalCourseFormat, Is.EqualTo("topics"));
            Assert.That(systemUnderTest.OriginalCourseFullname, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseShortname, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseStartDate, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseShortname, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseContextId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.OriginalSystemContextId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Details, Is.EqualTo(moodlebackupDetails));
            Assert.That(systemUnderTest.Contents, Is.EqualTo(moodlebackupContents));
            Assert.That(systemUnderTest.Settings, Is.EqualTo(moodlebackupSettings));
            
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlMoodleBackup_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupInformation = new MoodleBackupXmlInformation();
        
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.Detail = (moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.Section.Add(moodlebackupSection);
        
        var moodlebackupCourse = new MoodleBackupXmlCourse();

        var moodlebackupContents = new MoodleBackupXmlContents();

        
        MoodleBackupXmlActivities activities = new MoodleBackupXmlActivities();
        activities.Activity.Add(new MoodleBackupXmlActivity());
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        moodlebackupSettings.Setting.Add(moodlebackupSetting1);
        
        moodlebackupInformation.Details = moodlebackupDetails; 
        moodlebackupInformation.Settings = moodlebackupSettings; 
        moodlebackupInformation.Contents = moodlebackupContents;
        
        //Act
        var systemUnderTest = new MoodleBackupXmlMoodleBackup();
        systemUnderTest.Information = moodlebackupInformation;

        //Assert
        Assert.That(systemUnderTest.Information, Is.EqualTo(moodlebackupInformation));
        
    }
    
    [Test]
    public void MoodleBackupXmlMoodleBackup_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        //Arrange
        var moodlebackupInformation = new MoodleBackupXmlInformation();
        
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.Detail = (moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.Section.Add(moodlebackupSection);
        
        var moodlebackupCourse = new MoodleBackupXmlCourse();

        var moodlebackupContents = new MoodleBackupXmlContents();

        
        MoodleBackupXmlActivities activities = new MoodleBackupXmlActivities();
        activities.Activity.Add(new MoodleBackupXmlActivity());
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        moodlebackupSettings.Setting.Add(moodlebackupSetting1);
        
        moodlebackupInformation.Details = moodlebackupDetails; 
        moodlebackupInformation.Settings = moodlebackupSettings; 
        moodlebackupInformation.Contents = moodlebackupContents;
        
        var systemUnderTest = new MoodleBackupXmlMoodleBackup();
        systemUnderTest.Information = moodlebackupInformation;

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "moodle_backup.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }

}