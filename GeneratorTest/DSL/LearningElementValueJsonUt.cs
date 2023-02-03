using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class LearningElementValueJsonUt
{

    [Test]
    public void LearningElementValueJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
      
        //Act
        var learningElementValue = new LearningElementValueJson("points", "10");

        //Assert
        Assert.That(learningElementValue.Type, Is.EqualTo("points"));
        Assert.That(learningElementValue.Value, Is.EqualTo("10"));

    }
}