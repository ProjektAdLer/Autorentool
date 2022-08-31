using Generator.XmlClasses.Entities._activities.Lesson.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Lesson.xml;

[TestFixture]
public class ActivitiesLessonXmlPageUt
{
    [Test]
    public void ActivitiesLessonXmlPage_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        var lessonAnswer = new ActivitiesLessonXmlAnswer();
        var lessonAnswers = new ActivitiesLessonXmlAnswers();
        lessonAnswers.Answer = lessonAnswer;
        
        //Act
        var systemUnderTest = new ActivitiesLessonXmlPage();
        systemUnderTest.Answers = lessonAnswers;
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.PrevPageId, Is.EqualTo("0"));
            Assert.That(systemUnderTest.NextPageId, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Qtype, Is.EqualTo("20"));
            Assert.That(systemUnderTest.Qoption, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Layout, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Display, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Title, Is.EqualTo(""));
            Assert.That(systemUnderTest.Contents, Is.EqualTo(""));
            Assert.That(systemUnderTest.ContentsFormat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Answers, Is.EqualTo(lessonAnswers));
            Assert.That(systemUnderTest.Branches, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            
        });
        
    }
}