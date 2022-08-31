using Generator.XmlClasses.Entities._activities.Inforef.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Inforef.xml;

[TestFixture]
public class ActivitiesInforefXmlGradeItemUt
{
    [Test]
    public void ActivitiesInforefXmlGradeItem_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new ActivitiesInforefXmlGradeItem();
        
        //Assert
        Assert.That(systemUnderTest.Id, Is.EqualTo("1"));
    }
}