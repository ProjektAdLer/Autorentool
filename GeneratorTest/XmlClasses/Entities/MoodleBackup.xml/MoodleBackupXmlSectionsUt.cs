using Generator.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlSectionsUt
{
    [Test]
    public void MoodleBackupXmlSections_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupSection = new MoodleBackupXmlSection();

        var systemUnderTest = new MoodleBackupXmlSections();

        //Act
        systemUnderTest.Section.Add(moodlebackupSection);
        
        //Assert
        Assert.That(systemUnderTest.Section, Is.EquivalentTo(new List<MoodleBackupXmlSection> { moodlebackupSection }));
    }
}