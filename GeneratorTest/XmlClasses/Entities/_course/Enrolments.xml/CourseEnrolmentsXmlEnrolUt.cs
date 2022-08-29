using Generator.XmlClasses.Entities._course.Enrolments.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Enrolments.xml;

[TestFixture]
public class CourseEnrolmentsXmlEnrolUt
{
    [Test]
    public void CourseEnrolmentsXmlEnrol_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new CourseEnrolmentsXmlEnrol();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.EnrolMethod, Is.EqualTo(""));
            Assert.That(systemUnderTest.Status, Is.EqualTo(""));
            Assert.That(systemUnderTest.Name, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.EnrolPeriod, Is.EqualTo("0"));
            Assert.That(systemUnderTest.EnrolStartDate, Is.EqualTo("0"));
            Assert.That(systemUnderTest.EnrolEndDate, Is.EqualTo("0"));
            Assert.That(systemUnderTest.ExpiryNotify, Is.EqualTo("0"));
            Assert.That(systemUnderTest.ExpiryThreshold, Is.EqualTo("86400"));
            Assert.That(systemUnderTest.NotifyAll, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Password, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Cost, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Currency, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.RoleId, Is.EqualTo(""));
            Assert.That(systemUnderTest.CustomInt1, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt2, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt3, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt4, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt5, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt6, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt7, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomInt8, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomChar1, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomChar2, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomChar3, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText1, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText2, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText3, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CustomText4, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.UserEnrolments, Is.EqualTo(""));
            
        });
    }
}