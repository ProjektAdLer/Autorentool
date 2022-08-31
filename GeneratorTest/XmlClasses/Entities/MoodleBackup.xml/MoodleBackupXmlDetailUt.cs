using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlDetailUt
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
}