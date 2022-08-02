using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Groups.xml;

[XmlRoot(ElementName="groups")]
public class GroupsXmlGroups : IGroupsXmlGroups{


    public GroupsXmlGroups()
    {
        GroupingsList = new GroupsXmlGroupingsList();
    }
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "groups.xml");
    }
    
    [XmlElement(ElementName="GroupingsList")]
    public GroupsXmlGroupingsList GroupingsList { get; set; }
}