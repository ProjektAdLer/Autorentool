using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class ElementValueJsonUt
{

    [Test]
    public void ElementValueJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
      
        //Act
        var elementValue = new ElementValueJson("points", "10");

        //Assert
        Assert.That(elementValue.Type, Is.EqualTo("points"));
        Assert.That(elementValue.Value, Is.EqualTo("10"));

    }
}