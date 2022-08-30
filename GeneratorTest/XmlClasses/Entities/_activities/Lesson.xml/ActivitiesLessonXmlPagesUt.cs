using Generator.XmlClasses.Entities._activities.Lesson.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Lesson.xml;

[TestFixture]
public class ActivitiesLessonXmlPagesUt
{
    [Test]
    public void ActivitiesLessonXmlPages_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        var lessonAnswer = new ActivitiesLessonXmlAnswer();
        var lessonAnswers = new ActivitiesLessonXmlAnswers();
        lessonAnswers.Answer = lessonAnswer;
        var lessonPage = new ActivitiesLessonXmlPage();
        lessonPage.Answers = lessonAnswers;
        
        //Act
        var systemUnderTest = new ActivitiesLessonXmlPages();
        systemUnderTest.Page = lessonPage;
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Page, Is.EqualTo(lessonPage));
        });
        
    }
}