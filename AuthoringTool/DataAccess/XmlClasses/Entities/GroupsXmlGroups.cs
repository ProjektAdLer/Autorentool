using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="groups")]
public partial class GroupsXmlGroups : IGroupsXmlGroups{

    public void SetParameters(GroupsXmlGroupingsList? groupingsList)
    {
        GroupingsList = groupingsList;
    }
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "groups.xml");
    }
    
    [XmlElement(ElementName="GroupingsList")]
    public GroupsXmlGroupingsList? GroupingsList { get; set; }
}