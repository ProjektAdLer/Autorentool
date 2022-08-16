using AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlCourseUt
{
    
    [Test]
    public void MoodleBackupXmlCourse_StandardConstructor_AllParametersSet()
    {
        //Arrange

        //Act
        var systemUnderTest = new MoodleBackupXmlCourse();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CourseId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Title, Is.EqualTo(""));
            Assert.That(systemUnderTest.Directory, Is.EqualTo("course"));
        });
        
    }

}