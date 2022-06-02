using System.Collections.Generic;
using AuthoringTool.DataAccess.DSL;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class RequirementJsonUt
{
    [Test]
    public void RequirementJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var requirement = new RequirementJson();
        List<int> list = new List<int>();
        list.Add(1);
        list.Add(2);

            //Act
        requirement.type = "type";
        requirement.value = list;

        //Assert
        Assert.That(requirement.type, Is.EqualTo("type"));
        Assert.That(requirement.value, Is.EqualTo(list));
    }
}