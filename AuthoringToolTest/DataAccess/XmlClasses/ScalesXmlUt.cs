using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class ScalesXmlUt
{
    [Test]
    public void ScalesXmlScalesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var scalesScalesDefinition = new ScalesXmlScalesDefinition();
        
        //Act
        scalesScalesDefinition.SetParameters();
        
        //Assert
        Assert.That(scalesScalesDefinition, Is.EqualTo(scalesScalesDefinition));
    }
}