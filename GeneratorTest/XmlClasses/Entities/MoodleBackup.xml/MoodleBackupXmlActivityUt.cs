using AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlActivityUt
{
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
}