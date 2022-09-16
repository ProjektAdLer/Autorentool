using Generator.XmlClasses.Entities._course.Enrolments.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Enrolments.xml;

[TestFixture]
public class CourseEnrolmentsXmlEnrolsUt
{
    [Test]
    public void CourseEnrolmentsXmlEnrols_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var enrolmentsEnrol1 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol2 = new CourseEnrolmentsXmlEnrol();
        var enrolmentsEnrol3 = new CourseEnrolmentsXmlEnrol();
        var systemUnderTest = new CourseEnrolmentsXmlEnrols();
        
        //Act
        systemUnderTest.Enrol.Add(enrolmentsEnrol1);
        systemUnderTest.Enrol.Add(enrolmentsEnrol2);
        systemUnderTest.Enrol.Add(enrolmentsEnrol3);

        var expectedResult = new List<CourseEnrolmentsXmlEnrol?>();
        expectedResult.Add(enrolmentsEnrol1);
        expectedResult.Add(enrolmentsEnrol2);
        expectedResult.Add(enrolmentsEnrol3);
        
        //Assert
        Assert.That(systemUnderTest.Enrol, Is.EqualTo(expectedResult));
    }

}