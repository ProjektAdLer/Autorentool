using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlContentsUt
{
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


}