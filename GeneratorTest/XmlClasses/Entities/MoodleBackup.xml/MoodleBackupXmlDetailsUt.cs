using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlDetailsUt
{
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

}