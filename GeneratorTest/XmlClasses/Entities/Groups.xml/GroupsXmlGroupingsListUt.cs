using Generator.XmlClasses.Entities.Groups.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Groups.xml;

[TestFixture]
public class GroupsXmlGroupingsListUt
{
    [Test]
    public void GroupsXmlGroupingsList_StandardConstructor_AllParametersSet()
    {
        //Arrange
       
        //Act
        var systemUnderTest = new GroupsXmlGroupingsList();
        
        //Assert
        Assert.That(systemUnderTest.Groupings, Is.EqualTo(""));

    }
}