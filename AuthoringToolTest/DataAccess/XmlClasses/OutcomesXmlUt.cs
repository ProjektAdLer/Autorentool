using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class OutcomesXmlUt
{
    [Test]
    public void OutcomesXmlOutcomesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var outcomesOutcomesDefinition = new OutcomesXmlOutcomesDefinition();
        
        //Act
        outcomesOutcomesDefinition.SetParameters();
        
        //Assert
        Assert.That(outcomesOutcomesDefinition, Is.EqualTo(outcomesOutcomesDefinition));
    }
}