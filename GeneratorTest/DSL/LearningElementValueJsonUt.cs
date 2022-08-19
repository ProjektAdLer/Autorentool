using Generator.DSL;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class LearningElementValueJsonUt
{

    [Test]
    public void LearningElementValueJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
      
        //Act
        var learningElementValue = new LearningElementValueJson("type", "value");

        //Assert
        Assert.That(learningElementValue.Type, Is.EqualTo("type"));
        Assert.That(learningElementValue.Value, Is.EqualTo("value"));

    }
}