using AuthoringTool.DataAccess.DSL;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class IdentifierJsonUt
{
    [Test]
    public void IdentifierJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var ident = new IdentifierJson();
        
        //Act
        ident.type = "type";
        ident.value = "value";

        //Assert
        Assert.That(ident.type, Is.EqualTo("type"));
        Assert.That(ident.value, Is.EqualTo("value"));
    }
}