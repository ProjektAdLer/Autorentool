using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="groups")]
public partial class GroupsXmlGroups {
    [XmlElement(ElementName="GroupingsList")]
    public GroupsXmlGroupingsList GroupingsList { get; set; }
}