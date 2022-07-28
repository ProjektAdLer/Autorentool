using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="GroupingsList")]
public partial class GroupsXmlGroupingsList : IGroupsXmlGroupingsList
{

    public GroupsXmlGroupingsList()
    {
        Groupings = "";
    }
    
    [XmlElement(ElementName = "groupings")]
    public string Groupings { get; set; }
}