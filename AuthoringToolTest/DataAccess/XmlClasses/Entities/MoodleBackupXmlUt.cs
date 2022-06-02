using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class MoodleBackupXmlUt
{
    [Test]
    public void MoodleBackupXmlDetails_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        moodlebackupDetail.SetParameters("course", "moodle2", "1",
            "10", "1", "0", "36d63c7b4624cf6a79e0405be770974d");
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        
        //Act
        moodlebackupDetails.SetParameters(moodlebackupDetail);
        
        //Assert
        Assert.That(moodlebackupDetails.Detail, Is.EqualTo(moodlebackupDetail));
    }
    
    [Test]
    public void MoodleBackupXmlDetail_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        
        //Act
        moodlebackupDetail.SetParameters("course", "moodle2", "1",
            "10", "1", "0", "36d63c7b4624cf6a79e0405be770974d");
        
        //Assert
        Assert.That(moodlebackupDetail.Type, Is.EqualTo("course"));
        Assert.That(moodlebackupDetail.Format, Is.EqualTo("moodle2"));
        Assert.That(moodlebackupDetail.Interactive, Is.EqualTo("1"));
        Assert.That(moodlebackupDetail.Mode, Is.EqualTo("10"));
        Assert.That(moodlebackupDetail.Execution, Is.EqualTo("1"));
        Assert.That(moodlebackupDetail.Executiontime, Is.EqualTo("0"));
        Assert.That(moodlebackupDetail.Backup_id, Is.EqualTo("36d63c7b4624cf6a79e0405be770974d"));
    }
    
    [Test]
    public void MoodleBackupXmlSections_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        List<MoodleBackupXmlSection>? list = new List<MoodleBackupXmlSection>();

        //Act
        list.Add(moodlebackupSection);
        moodlebackupSections.SetParameters(list);
        
        //Assert
        Assert.That(moodlebackupSections.Section, Is.EqualTo(list));
    }
    
    [Test]
    public void MoodleBackupXmlSection_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSection = new MoodleBackupXmlSection();

        //Act
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupSection.Sectionid, Is.EqualTo("160"));
            Assert.That(moodlebackupSection.Title, Is.EqualTo("1"));
            Assert.That(moodlebackupSection.Directory, Is.EqualTo("sections/section_160"));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlCourse_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupCourse = new MoodleBackupXmlCourse();

        //Act
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupCourse.Courseid, Is.EqualTo("53"));
            Assert.That(moodlebackupCourse.Title, Is.EqualTo("XML_LK"));
            Assert.That(moodlebackupCourse.Directory, Is.EqualTo("course"));
        });
        
    }

    [Test]
    public void MoodleBackupXmlActivity_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var activity = new MoodleBackupXmlActivity();
        
        //Act
        activity.SetParameters("1", "1", "name",
            "title", "dir");
        
        //Assert
        Assert.That(activity.Moduleid, Is.EqualTo("1"));
        Assert.That(activity.Sectionid, Is.EqualTo("1"));
        Assert.That(activity.Modulename, Is.EqualTo("name"));
        Assert.That(activity.Title, Is.EqualTo("title"));
        Assert.That(activity.Directory, Is.EqualTo("dir"));
    }
    
    [Test]
    public void MoodleBackupXmlActivities_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var activity = new MoodleBackupXmlActivity();
        activity.SetParameters("1", "1", "name",
            "title", "dir");
        var activities = new MoodleBackupXmlActivities();
        List<MoodleBackupXmlActivity> list = new List<MoodleBackupXmlActivity>();
        list.Add(activity);
        
        //Act
        activities.SetParameters(list);
        
        //Assert
        Assert.That(activities.Activity, Is.EqualTo(list));


    }
    
    
    [Test]
    public void MoodleBackupXmlContents_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupContents = new MoodleBackupXmlContents();
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        List<MoodleBackupXmlSection>? list = new List<MoodleBackupXmlSection>();
        list.Add(moodlebackupSection);
        moodlebackupSections.SetParameters(list);
        List<MoodleBackupXmlActivity>? moodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        moodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity());
        moodleBackupXmlActivityList[moodleBackupXmlActivityList.Count-1].SetParameters("learningElementId", 
            "learningElementId", "learningElementType",
            "learningElementName", "activities/"+"learningElementType"+"_"+"learningElementId");
        MoodleBackupXmlActivities activities = new MoodleBackupXmlActivities();
        activities.SetParameters(moodleBackupXmlActivityList);
        
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");

        //Act
        moodlebackupContents.SetParameters(activities,moodlebackupSections, moodlebackupCourse);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupContents.Sections, Is.EqualTo(moodlebackupSections));
            Assert.That(moodlebackupContents.Course, Is.EqualTo(moodlebackupCourse));
            Assert.That(moodlebackupContents.Activities, Is.EqualTo(activities));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSetting_SetParametersShort_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSetting = new MoodleBackupXmlSetting();
        
        //Act
        moodlebackupSetting.SetParametersSetting("filename", "C#_XML_Created_Backup.mbz", "1");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupSetting.Level, Is.EqualTo("filename"));
            Assert.That(moodlebackupSetting.Name, Is.EqualTo("C#_XML_Created_Backup.mbz"));
            Assert.That(moodlebackupSetting.Value, Is.EqualTo("1"));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSetting_SetParametersFull_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSetting = new MoodleBackupXmlSetting();
        
        //Act
        moodlebackupSetting.SetParametersActivity("section_160_userinfo", "0", "section", "section_160");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupSetting.Level, Is.EqualTo("section_160_userinfo"));
            Assert.That(moodlebackupSetting.Activity, Is.EqualTo("0"));
            Assert.That(moodlebackupSetting.Name, Is.EqualTo("section"));
            Assert.That(moodlebackupSetting.Value, Is.EqualTo("section_160"));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSettings_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        
        //Act
        List<MoodleBackupXmlSetting?>? list = new List<MoodleBackupXmlSetting?>();
        moodlebackupSettings.SetParameters(list);
        
        //Assert
        Assert.That(moodlebackupSettings.Setting, Is.EqualTo(new List<MoodleBackupXmlSetting>()));
    }
    
    [Test]
    public void MoodleBackupXmlInformation_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupInformation = new MoodleBackupXmlInformation();
        
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        moodlebackupDetail.SetParameters("course", "moodle2", "1",
            "10", "1", "0", "36d63c7b4624cf6a79e0405be770974d");
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.SetParameters(moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        List<MoodleBackupXmlSection>? listSection = new List<MoodleBackupXmlSection>();
        listSection.Add(moodlebackupSection);
        moodlebackupSections.SetParameters(listSection);
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        var moodlebackupContents = new MoodleBackupXmlContents();
        List<MoodleBackupXmlActivity>? moodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        moodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity());
        moodleBackupXmlActivityList[moodleBackupXmlActivityList.Count-1].SetParameters("learningElementId", 
            "learningElementId", "learningElementType",
            "learningElementName", "activities/"+"learningElementType"+"_"+"learningElementId");
        MoodleBackupXmlActivities activities = new MoodleBackupXmlActivities();
        activities.SetParameters(moodleBackupXmlActivityList);
        
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        moodlebackupSetting1.SetParametersSetting("filename", "C#_XML_Created_Backup.mbz", "1");
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        List<MoodleBackupXmlSetting?>? listSetting = new List<MoodleBackupXmlSetting?>();
        listSetting.Add(moodlebackupSetting1);
        moodlebackupSettings.SetParameters(listSetting);
        
        //Act
        moodlebackupInformation.SetParameters("C#_AuthoringTool_Created_Backup.mbz", "2021051703",
            "3.11.3 (Build: 20210913)", "2021051700", "3.11","currentTime", "0",
            "1","0","https://moodle.cluuub.xyz",
            "c9629ccd3c092478330b78bdf4dcdb18","1","topics", 
            "learningWorld.identifier.value.ToString()", "learningWorld.identifier.value.ToString()", 
            "currentTime", "2221567452", "1", "1", 
            moodlebackupDetails as MoodleBackupXmlDetails, moodlebackupContents as MoodleBackupXmlContents, 
            moodlebackupSettings as MoodleBackupXmlSettings);

        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupInformation.Name, Is.EqualTo("C#_AuthoringTool_Created_Backup.mbz"));
            Assert.That(moodlebackupInformation.Moodle_version, Is.EqualTo("2021051703"));
            Assert.That(moodlebackupInformation.Moodle_release, Is.EqualTo("3.11.3 (Build: 20210913)"));
            Assert.That(moodlebackupInformation.Backup_version, Is.EqualTo("2021051700"));
            Assert.That(moodlebackupInformation.Backup_release, Is.EqualTo("3.11"));
            Assert.That(moodlebackupInformation.Backup_date, Is.EqualTo("currentTime"));
            Assert.That(moodlebackupInformation.Mnet_remoteusers, Is.EqualTo("0"));
            Assert.That(moodlebackupInformation.Include_files, Is.EqualTo("1"));
            Assert.That(moodlebackupInformation.Include_file_references_to_external_content, Is.EqualTo("0"));
            Assert.That(moodlebackupInformation.Original_wwwroot, Is.EqualTo("https://moodle.cluuub.xyz"));
            Assert.That(moodlebackupInformation.Original_site_identifier_hash, Is.EqualTo("c9629ccd3c092478330b78bdf4dcdb18"));
            Assert.That(moodlebackupInformation.Original_course_id, Is.EqualTo("1"));
            Assert.That(moodlebackupInformation.Original_course_format, Is.EqualTo("topics"));
            Assert.That(moodlebackupInformation.Original_course_fullname, Is.EqualTo("learningWorld.identifier.value.ToString()"));
            Assert.That(moodlebackupInformation.Original_course_shortname, Is.EqualTo("learningWorld.identifier.value.ToString()"));
            Assert.That(moodlebackupInformation.Original_course_startdate, Is.EqualTo("currentTime"));
            Assert.That(moodlebackupInformation.Original_course_shortname, Is.EqualTo("learningWorld.identifier.value.ToString()"));
            Assert.That(moodlebackupInformation.Original_course_contextid, Is.EqualTo("1"));
            Assert.That(moodlebackupInformation.Original_system_contextid, Is.EqualTo("1"));
            Assert.That(moodlebackupInformation.Details, Is.EqualTo(moodlebackupDetails));
            Assert.That(moodlebackupInformation.Contents, Is.EqualTo(moodlebackupContents));
            Assert.That(moodlebackupInformation.Settings, Is.EqualTo(moodlebackupSettings));
            
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlMoodleBackup_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupInformation = new MoodleBackupXmlInformation();
        
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        moodlebackupDetail.SetParameters("course", "moodle2", "1",
            "10", "1", "0", "36d63c7b4624cf6a79e0405be770974d");
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.SetParameters(moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        List<MoodleBackupXmlSection>? listSection = new List<MoodleBackupXmlSection>();
        listSection.Add(moodlebackupSection);
        moodlebackupSections.SetParameters(listSection);
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        var moodlebackupContents = new MoodleBackupXmlContents();
        List<MoodleBackupXmlActivity>? moodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        moodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity());
        moodleBackupXmlActivityList[moodleBackupXmlActivityList.Count-1].SetParameters("learningElementId", 
            "learningElementId", "learningElementType",
            "learningElementName", "activities/"+"learningElementType"+"_"+"learningElementId");
        MoodleBackupXmlActivities activities = new MoodleBackupXmlActivities();
        activities.SetParameters(moodleBackupXmlActivityList);
        
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        moodlebackupSetting1.SetParametersSetting("filename", "C#_XML_Created_Backup.mbz", "1");
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        List<MoodleBackupXmlSetting?>? listSetting = new List<MoodleBackupXmlSetting?>();
        listSetting.Add(moodlebackupSetting1);
        moodlebackupSettings.SetParameters(listSetting);
        
        //Act
        moodlebackupInformation.SetParameters("C#_AuthoringTool_Created_Backup.mbz", "2021051703",
            "3.11.3 (Build: 20210913)", "2021051700", "3.11","currentTime", "0",
            "1","0","https://moodle.cluuub.xyz",
            "c9629ccd3c092478330b78bdf4dcdb18","1","topics", 
            "learningWorld.identifier.value.ToString()", "learningWorld.identifier.value.ToString()", 
            "currentTime", "2221567452", "1", "1", 
            moodlebackupDetails as MoodleBackupXmlDetails, moodlebackupContents as MoodleBackupXmlContents, 
            moodlebackupSettings as MoodleBackupXmlSettings);
        
        var moodlebackup = new MoodleBackupXmlMoodleBackup();
        
        //Act
        moodlebackup.SetParameters(moodlebackupInformation);

        
        //Assert
        Assert.That(moodlebackup.Information, Is.EqualTo(moodlebackupInformation));
        
    }
    
    [Test]
    public void MoodleBackupXmlMoodleBackup_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
               var moodlebackupInformation = new MoodleBackupXmlInformation();
        
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        moodlebackupDetail.SetParameters("course", "moodle2", "1",
            "10", "1", "0", "36d63c7b4624cf6a79e0405be770974d");
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.SetParameters(moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        List<MoodleBackupXmlSection>? listSection = new List<MoodleBackupXmlSection>();
        listSection.Add(moodlebackupSection);
        moodlebackupSections.SetParameters(listSection);
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        var moodlebackupContents = new MoodleBackupXmlContents();
        List<MoodleBackupXmlActivity>? moodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        moodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity());
        moodleBackupXmlActivityList[moodleBackupXmlActivityList.Count-1].SetParameters("learningElementId", 
            "learningElementId", "learningElementType",
            "learningElementName", "activities/"+"learningElementType"+"_"+"learningElementId");
        MoodleBackupXmlActivities activities = new MoodleBackupXmlActivities();
        activities.SetParameters(moodleBackupXmlActivityList);
        
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        moodlebackupSetting1.SetParametersSetting("filename", "C#_XML_Created_Backup.mbz", "1");
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        List<MoodleBackupXmlSetting?>? listSetting = new List<MoodleBackupXmlSetting?>();
        listSetting.Add(moodlebackupSetting1);
        moodlebackupSettings.SetParameters(listSetting);
        
        //Act
        moodlebackupInformation.SetParameters("C#_AuthoringTool_Created_Backup.mbz", "2021051703",
            "3.11.3 (Build: 20210913)", "2021051700", "3.11","currentTime", "0",
            "1","0","https://moodle.cluuub.xyz",
            "c9629ccd3c092478330b78bdf4dcdb18","1","topics", 
            "learningWorld.identifier.value.ToString()", "learningWorld.identifier.value.ToString()", 
            "currentTime", "2221567452", "1", "1", 
            moodlebackupDetails as MoodleBackupXmlDetails, moodlebackupContents as MoodleBackupXmlContents, 
            moodlebackupSettings as MoodleBackupXmlSettings);

        var moodlebackup = new MoodleBackupXmlMoodleBackup();
        moodlebackup.SetParameters(moodlebackupInformation);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        moodlebackup.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "moodle_backup.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }

}