using Generator.XmlClasses.Entities._activities.H5PActivity.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.H5pActivity.xml;

[TestFixture]
public class ActivitiesH5PActivityXmlH5PActivityUt
{
    [Test]
    public void ActivitiesH5pActivityXmlH5pActivity_StandardConstructor_AllParametersSet()
    {
        //Arrange
        // ReSharper disable once InconsistentNaming
        
        //Act
        var systemUnderTest = new ActivitiesH5PActivityXmlH5PActivity();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Intro, Is.EqualTo(""));
            Assert.That(systemUnderTest.Introformat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Grade, Is.EqualTo("100"));
            Assert.That(systemUnderTest.Displayoptions, Is.EqualTo("15"));
            Assert.That(systemUnderTest.Enabletracking, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Grademethod, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Reviewmode, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Attempts, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
    }
}