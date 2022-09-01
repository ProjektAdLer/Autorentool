using Generator.XmlClasses.Entities._activities.Lesson.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Lesson.xml;

[TestFixture]
public class ActivitiesLessonXmlAnswerUt
{
    [Test]
    public void ActivitiesLessonXmlAnswer_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new ActivitiesLessonXmlAnswer();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.JumpTo, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Grade, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Score, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Flags, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.AnswerText, Is.EqualTo(""));
            Assert.That(systemUnderTest.Response, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Answerformat, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Responseformat, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Attempts, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
        
    }
}