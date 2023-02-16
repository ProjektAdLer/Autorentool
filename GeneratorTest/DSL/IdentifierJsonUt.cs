using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class IdentifierJsonUt
{
    [Test]
    public void IdentifierJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        
        
        //Act
        var ident = new LmsElementIdentifierJson("type", "value");

        //Assert
        Assert.That(ident.Type, Is.EqualTo("type"));
        Assert.That(ident.Value, Is.EqualTo("value"));
    }
}