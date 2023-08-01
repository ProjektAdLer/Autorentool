using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.Lesson.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Lesson.xml;

[TestFixture]
public class ActivitiesLessonXmlActivityUt
{
    [Test]
    public void ActivitiesLessonXmlActivity_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var lessonAnswer = new ActivitiesLessonXmlAnswer();
        var lessonAnswers = new ActivitiesLessonXmlAnswers();
        lessonAnswers.Answer = lessonAnswer;
        var lessonPage = new ActivitiesLessonXmlPage();
        lessonPage.Answers = lessonAnswers;
        var lessonPages = new ActivitiesLessonXmlPages();
        lessonPages.Page = lessonPage;
        var lessonLesson = new ActivitiesLessonXmlLesson();
        lessonLesson.Pages = lessonPages;

        //Act
        var systemUnderTest = new ActivitiesLessonXmlActivity();
        systemUnderTest.Lesson = lessonLesson;

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Lesson, Is.EqualTo(lessonLesson));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleId, Is.EqualTo(""));
            Assert.That(systemUnderTest.ModuleName, Is.EqualTo("lesson"));
            Assert.That(systemUnderTest.ContextId, Is.EqualTo("1"));
        });
    }

    [Test]
    public void ActivitiesLessonXmlActivity_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        var activityName = "lesson";
        var moduleId = "1";
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities",
            activityName + "_" + moduleId));

        var lessonAnswer = new ActivitiesLessonXmlAnswer();
        var lessonAnswers = new ActivitiesLessonXmlAnswers();
        lessonAnswers.Answer = lessonAnswer;
        var lessonPage = new ActivitiesLessonXmlPage();
        lessonPage.Answers = lessonAnswers;
        var lessonPages = new ActivitiesLessonXmlPages();
        lessonPages.Page = lessonPage;
        var lessonLesson = new ActivitiesLessonXmlLesson();
        lessonLesson.Pages = lessonPages;

        var systemUnderTest = new ActivitiesLessonXmlActivity();
        systemUnderTest.Lesson = lessonLesson;


        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize(activityName, moduleId);

        //Assert
        var pathXmlFile = Path.Join(currWorkDir, "XMLFilesForExport", "activities",
            activityName + "_" + moduleId, "lesson.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}