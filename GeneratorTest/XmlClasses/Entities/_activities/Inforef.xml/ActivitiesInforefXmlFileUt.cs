using Generator.XmlClasses.Entities._activities.Inforef.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Inforef.xml;

[TestFixture]
public class ActivitiesInforefXmlFileUt
{
    [Test]
    public void ActivitiesInforefXmlFile_StandardConstructor_AllParametersSet()
    {
        //Arrange

        //Act
        var systemUderTest = new ActivitiesInforefXmlFile();

        //Assert
        Assert.That(systemUderTest.Id, Is.EqualTo(""));
    }
}