using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlSectionUt
{
     
    [Test]
    public void MoodleBackupXmlSection_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new MoodleBackupXmlSection();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SectionId, Is.EqualTo(""));
            Assert.That(systemUnderTest.Title, Is.EqualTo(""));
            Assert.That(systemUnderTest.Directory, Is.EqualTo(""));
        });
        
    }
}