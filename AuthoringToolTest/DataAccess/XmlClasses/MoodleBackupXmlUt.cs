using System.Collections.Generic;
using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class MoodleBackupXmlUt
{
    [Test]
    public void MoodleBackupXmlDetails_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        moodlebackupDetail.SetParameters("6a4e8e833791eb72e5f3ee2227ee1b74");
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
        moodlebackupDetail.SetParameters("6a4e8e833791eb72e5f3ee2227ee1b74");
        
        //Assert
        Assert.That(moodlebackupDetail.Backup_id, Is.EqualTo("6a4e8e833791eb72e5f3ee2227ee1b74"));
    }
    
    [Test]
    public void MoodleBackupXmlSections_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        
        //Act
        moodlebackupSections.SetParameters(moodlebackupSection);
        
        //Assert
        Assert.That(moodlebackupSections.Section, Is.EqualTo(moodlebackupSection));
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
    public void MoodleBackupXmlContents_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupContents = new MoodleBackupXmlContents();
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.SetParameters(moodlebackupSection);
        
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");

        //Act
        moodlebackupContents.SetParameters(moodlebackupSections, moodlebackupCourse);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupContents.Sections, Is.EqualTo(moodlebackupSections));
            Assert.That(moodlebackupContents.Course, Is.EqualTo(moodlebackupCourse));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSetting_SetParametersShort_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSetting = new MoodleBackupXmlSetting();
        
        //Act
        moodlebackupSetting.SetParametersShort("filename", "C#_XML_Created_Backup.mbz");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupSetting.Name, Is.EqualTo("filename"));
            Assert.That(moodlebackupSetting.Value, Is.EqualTo("C#_XML_Created_Backup.mbz"));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSetting_SetParametersFull_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSetting = new MoodleBackupXmlSetting();
        
        //Act
        moodlebackupSetting.SetParametersFull("section_160_userinfo", "0", "section", "section_160");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupSetting.Name, Is.EqualTo("section_160_userinfo"));
            Assert.That(moodlebackupSetting.Value, Is.EqualTo("0"));
            Assert.That(moodlebackupSetting.Level, Is.EqualTo("section"));
            Assert.That(moodlebackupSetting.Section, Is.EqualTo("section_160"));
        });
        
    }
    
    [Test]
    public void MoodleBackupXmlSettings_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        
        //Act
        moodlebackupSettings.SetParameters();
        
        //Assert
        Assert.That(moodlebackupSettings.Setting, Is.EqualTo(new List<MoodleBackupXmlSetting>()));
    }
    
    [Test]
    public void MoodleBackupXmlInformation_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var moodlebackupInformation = new MoodleBackupXmlInformation();
        
        var moodlebackupDetail = new MoodleBackupXmlDetail();
        moodlebackupDetail.SetParameters("6a4e8e833791eb72e5f3ee2227ee1b74");
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.SetParameters(moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.SetParameters(moodlebackupSection);
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        var moodlebackupContents = new MoodleBackupXmlContents();
        moodlebackupContents.SetParameters(moodlebackupSections, moodlebackupCourse);
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        moodlebackupSetting1.SetParametersShort("filename", "C#_XML_Created_Backup.mbz");
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        moodlebackupSettings.SetParameters();
        moodlebackupSettings.Setting.Add(moodlebackupSetting1);
        
        //Act
        moodlebackupInformation.SetParameters("C#_XML_Created_Backup.mbz", 
            "53", "topics", "XML_Leerer Kurs", 
            "XML_LK", "286", "1", "1645484400", 
            "1677020400", moodlebackupDetails, moodlebackupContents, moodlebackupSettings);

        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(moodlebackupInformation.Name, Is.EqualTo("C#_XML_Created_Backup.mbz"));
            Assert.That(moodlebackupInformation.Original_course_id, Is.EqualTo("53"));
            Assert.That(moodlebackupInformation.Original_course_format, Is.EqualTo("topics"));
            Assert.That(moodlebackupInformation.Original_course_fullname, Is.EqualTo("XML_Leerer Kurs"));
            Assert.That(moodlebackupInformation.Original_course_shortname, Is.EqualTo("XML_LK"));
            Assert.That(moodlebackupInformation.Original_course_contextid, Is.EqualTo("286"));
            Assert.That(moodlebackupInformation.Original_system_contextid, Is.EqualTo("1"));
            Assert.That(moodlebackupInformation.Original_course_startdate, Is.EqualTo("1645484400"));
            Assert.That(moodlebackupInformation.Original_course_enddate, Is.EqualTo("1677020400"));
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
        moodlebackupDetail.SetParameters("6a4e8e833791eb72e5f3ee2227ee1b74");
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.SetParameters(moodlebackupDetail);
        
        var moodlebackupSection = new MoodleBackupXmlSection();
        moodlebackupSection.SetParameters("160", "1", "sections/section_160");
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.SetParameters(moodlebackupSection);
        var moodlebackupCourse = new MoodleBackupXmlCourse();
        moodlebackupCourse.SetParameters("53", "XML_LK", "course");
        var moodlebackupContents = new MoodleBackupXmlContents();
        moodlebackupContents.SetParameters(moodlebackupSections, moodlebackupCourse);
        
        
        var moodlebackupSetting1 = new MoodleBackupXmlSetting();
        moodlebackupSetting1.SetParametersShort("filename", "C#_XML_Created_Backup.mbz");
        var moodlebackupSettings = new MoodleBackupXmlSettings();
        moodlebackupSettings.SetParameters();
        moodlebackupSettings.Setting.Add(moodlebackupSetting1);
        
        
        moodlebackupInformation.SetParameters("C#_XML_Created_Backup.mbz", 
            "53", "topics", "XML_Leerer Kurs", 
            "XML_LK", "286", "1", "1645484400", 
            "1677020400", moodlebackupDetails, moodlebackupContents, moodlebackupSettings);
        
        var moodlebackup = new MoodleBackupXmlMoodleBackup();
        
        //Act
        moodlebackup.SetParameters(moodlebackupInformation);

        
        //Assert
        Assert.That(moodlebackup.Information, Is.EqualTo(moodlebackupInformation));
        
    }
    
    [Test]
    public void OutcomesXmlOutcomesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var outcomesOutcomesDefinition = new OutcomesXmlOutcomesDefinition();
        
        //Act
        outcomesOutcomesDefinition.SetParameters();
        
        //Assert
        Assert.That(outcomesOutcomesDefinition, Is.EqualTo(outcomesOutcomesDefinition));
    }
    
    [Test]
    public void QuestionsXmlQuestionsCategories_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var questionsQuestionsCategories = new QuestionsXmlQuestionsCategories();
        
        //Act
        questionsQuestionsCategories.SetParameters();
        
        //Assert
        Assert.That(questionsQuestionsCategories, Is.EqualTo(questionsQuestionsCategories));
    }
    
    [Test]
    public void RolesXmlRolesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var rolesRole = new RolesXmlRole();
        rolesRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        var rolesRolesDefinition = new RolesXmlRolesDefinition();
        rolesRolesDefinition.SetParameters(rolesRole);
        
        //Act
        rolesRolesDefinition.SetParameters(rolesRole);
        
        //Assert
        Assert.That(rolesRolesDefinition.Role, Is.EqualTo(rolesRole));
        
    }
    
    [Test]
    public void RolesXmlRole_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var rolesRole = new RolesXmlRole();

        //Act
        rolesRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(rolesRole.Name, Is.EqualTo(""));
            Assert.That(rolesRole.Description, Is.EqualTo(""));
            Assert.That(rolesRole.Id, Is.EqualTo("5"));
            Assert.That(rolesRole.Shortname, Is.EqualTo("student"));
            Assert.That(rolesRole.Nameincourse, Is.EqualTo("$@NULL@$"));
            Assert.That(rolesRole.Sortorder, Is.EqualTo("5"));
            Assert.That(rolesRole.Archetype, Is.EqualTo("student"));
        });
    }
    
    [Test]
    public void ScalesXmlScalesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var scalesScalesDefinition = new ScalesXmlScalesDefinition();
        
        //Act
        scalesScalesDefinition.SetParameters();
        
        //Assert
        Assert.That(scalesScalesDefinition, Is.EqualTo(scalesScalesDefinition));
    }
    
    
}