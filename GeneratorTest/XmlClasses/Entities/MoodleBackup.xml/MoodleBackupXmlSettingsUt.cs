using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlSettingsUt
{
    
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
}