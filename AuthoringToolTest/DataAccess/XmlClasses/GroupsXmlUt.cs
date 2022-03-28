using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class GroupsXmlUt
{

    [Test]
    public void GroupsXmlGroups_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var groupingsList = new GroupsXmlGroupingsList();
        groupingsList.SetParameters("");

        var groups = new GroupsXmlGroups();
        
        //Act
        groups.SetParameters(groupingsList);
        
        //Assert
        Assert.That(groups.GroupingsList, Is.EqualTo(groupingsList));
    }
    
    [Test]
    public void GroupsXmlGroupingsList_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var groupingsList = new GroupsXmlGroupingsList();
        
        //Act
        groupingsList.SetParameters("");
        
        //Assert
        Assert.That(groupingsList.Groupings, Is.EqualTo(""));

    }
}
    


