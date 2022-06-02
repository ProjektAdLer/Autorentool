using AuthoringTool.DataAccess.DSL;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class LearningElementValueJsonUt
{

    [Test]
    public void LearningElementValueJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var learningElementValue = new LearningElementValueJson();
        
        //Act
        learningElementValue.type = "type";
        learningElementValue.value = "value";

        //Assert
        Assert.That(learningElementValue.type, Is.EqualTo("type"));
        Assert.That(learningElementValue.value, Is.EqualTo("value"));

    }
}