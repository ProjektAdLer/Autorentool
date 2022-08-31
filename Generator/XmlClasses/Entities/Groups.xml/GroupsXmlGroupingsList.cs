using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Groups.xml;

[XmlRoot(ElementName="GroupingsList")]
public class GroupsXmlGroupingsList : IGroupsXmlGroupingsList
{

    public GroupsXmlGroupingsList()
    {
        Groupings = "";
    }
    
    [XmlElement(ElementName = "groupings")]
    public string Groupings { get; set; }
}