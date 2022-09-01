using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class RequirementJsonUt
{
    [Test]
    public void RequirementJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        
        List<int> list = new List<int>();
        list.Add(1);
        list.Add(2);

            //Act
        var requirement = new RequirementJson("type", list);


        //Assert
        Assert.That(requirement.Type, Is.EqualTo("type"));
        Assert.That(requirement.Value, Is.EqualTo(list));
    }
}