using Generator.XmlClasses.Entities._activities.Lesson.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Lesson.xml;

[TestFixture]
public class ActivitiesLessonXmlLessonUt
{
    [Test]
    public void ActivitiesLessonXmlLesson_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var lessonAnswer = new ActivitiesLessonXmlAnswer();
        var lessonAnswers = new ActivitiesLessonXmlAnswers();
        lessonAnswers.Answer = lessonAnswer;
        var lessonPage = new ActivitiesLessonXmlPage();
        lessonPage.Answers = lessonAnswers;
        var lessonPages = new ActivitiesLessonXmlPages();
        lessonPages.Page = lessonPage;
        
        //Act
        var systemUnderTest = new ActivitiesLessonXmlLesson();
        systemUnderTest.Pages = lessonPages;
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Course, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Intro, Is.EqualTo(""));
            Assert.That(systemUnderTest.IntroFormat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Practice, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Modattempts, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Usepassword, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Password, Is.EqualTo(""));
            Assert.That(systemUnderTest.Dependency, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Conditions, Is.EqualTo(""));
            Assert.That(systemUnderTest.Grade, Is.EqualTo("100"));
            Assert.That(systemUnderTest.Custom, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Ongoing, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Usemaxgrade, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Maxanswers, Is.EqualTo("5"));
            Assert.That(systemUnderTest.MaxAttempts, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Review, Is.EqualTo("0"));
            Assert.That(systemUnderTest.NextPagedefault, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Feedback, Is.EqualTo("0"));
            Assert.That(systemUnderTest.MinQuestions, Is.EqualTo("0"));
            Assert.That(systemUnderTest.MaxPages, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Timelimit, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Retake, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Activitylink, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Mediafile, Is.EqualTo(""));
            Assert.That(systemUnderTest.Mediaheight, Is.EqualTo("480"));
            Assert.That(systemUnderTest.Mediawidth, Is.EqualTo("640"));
            Assert.That(systemUnderTest.Mediaclose, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Slideshow, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Width, Is.EqualTo("480"));
            Assert.That(systemUnderTest.Height, Is.EqualTo("640"));
            Assert.That(systemUnderTest.Bgcolor, Is.EqualTo("#FFFFF"));
            Assert.That(systemUnderTest.Displayleft, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Displayleftif, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Progressbar, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Available, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Deadline, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.CompletionEndReached, Is.EqualTo("0"));
            Assert.That(systemUnderTest.CompletionTimeSpent, Is.EqualTo("0"));
            Assert.That(systemUnderTest.AllowOfflineAttempts, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Pages, Is.EqualTo(lessonPages));
            Assert.That(systemUnderTest.Grades, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timers, Is.EqualTo(""));
            Assert.That(systemUnderTest.Overrides, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
        
    }

}