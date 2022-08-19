using Generator.XmlClasses.Entities._activities.Lesson.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Lesson.xml;

[TestFixture]
public class ActivitiesLessonXmlAnswersUt
{
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
}