﻿using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlMoodleBackupUt
{
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