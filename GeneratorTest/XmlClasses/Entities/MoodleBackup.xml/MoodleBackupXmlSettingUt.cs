using AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlSettingUt
{
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
}