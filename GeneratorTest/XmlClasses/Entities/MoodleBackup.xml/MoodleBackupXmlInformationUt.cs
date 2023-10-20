using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlInformationUt
{
    [Test]
    public void MoodleBackupXmlInformation_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var mockContextId = 12345;
        var systemUnderTest = new MoodleBackupXmlInformation(mockContextId);

        var moodlebackupDetail = new MoodleBackupXmlDetail();
        var moodlebackupDetails = new MoodleBackupXmlDetails();
        moodlebackupDetails.Detail = (moodlebackupDetail);

        var moodlebackupSection = new MoodleBackupXmlSection();
        var moodlebackupSections = new MoodleBackupXmlSections();
        moodlebackupSections.Section.Add(moodlebackupSection);


        var moodlebackupContents = new MoodleBackupXmlContents();


        var activities = new MoodleBackupXmlActivities();
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
            Assert.That(systemUnderTest.MoodleVersion, Is.EqualTo("2022112805"));
            Assert.That(systemUnderTest.MoodleRelease, Is.EqualTo("4.1.5 (Build: 20230814)"));
            Assert.That(systemUnderTest.BackupVersion, Is.EqualTo("2022112805"));
            Assert.That(systemUnderTest.BackupRelease, Is.EqualTo("4.1"));
            Assert.That(systemUnderTest.BackupDate, Is.EqualTo(""));
            Assert.That(systemUnderTest.MnetRemoteUsers, Is.EqualTo("0"));
            Assert.That(systemUnderTest.IncludeFiles, Is.EqualTo("1"));
            Assert.That(systemUnderTest.IncludeFileReferencesToExternalContent, Is.EqualTo("0"));
            Assert.That(systemUnderTest.OriginalWwwRoot, Is.EqualTo("https://moodle.projekt-adler.eu"));
            Assert.That(systemUnderTest.OriginalSiteIdentifierHash, Is.EqualTo("c9629ccd3c092478330b78bdf4dcdb18"));
            Assert.That(systemUnderTest.OriginalCourseId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.OriginalCourseFormat, Is.EqualTo("topics"));
            Assert.That(systemUnderTest.OriginalCourseFullname, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseShortname, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseStartDate, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseShortname, Is.EqualTo(""));
            Assert.That(systemUnderTest.OriginalCourseContextId, Is.EqualTo("12345"));
            Assert.That(systemUnderTest.OriginalSystemContextId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Details, Is.EqualTo(moodlebackupDetails));
            Assert.That(systemUnderTest.Contents, Is.EqualTo(moodlebackupContents));
            Assert.That(systemUnderTest.Settings, Is.EqualTo(moodlebackupSettings));
        });
    }
}