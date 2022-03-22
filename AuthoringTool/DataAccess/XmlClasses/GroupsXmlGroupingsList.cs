using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="GroupingsList")]
public partial class GroupsXmlGroupingsList
{
    [XmlElement(ElementName = "groupings")]
    public string Groupings = "";
}