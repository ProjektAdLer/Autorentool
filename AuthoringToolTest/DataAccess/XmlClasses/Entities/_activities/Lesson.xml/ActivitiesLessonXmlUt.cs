using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities._activities.Lesson.xml;

[TestFixture]
public class ActivitiesLessonXmlUt
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

    [Test]
    public void ActivitiesLessonXmlAnswers_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var lessonAnswer = new ActivitiesLessonXmlAnswer();
        // Act
        var systemUnderTest = new ActivitiesLessonXmlAnswers();
        systemUnderTest.Answer = lessonAnswer;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Answer, Is.EqualTo(lessonAnswer));
        }); 

    }

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
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        string activityName = "lesson";
        string moduleId = "1";
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", activityName + "_" + moduleId));

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