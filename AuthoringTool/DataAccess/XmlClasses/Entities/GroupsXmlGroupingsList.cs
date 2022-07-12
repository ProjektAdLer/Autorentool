using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="GroupingsList")]
public partial class GroupsXmlGroupingsList : IGroupsXmlGroupingsList
{
    public void SetParameters(string groupings)
    {
        Groupings = groupings;
    }

    [XmlElement(ElementName = "groupings")]
    public string Groupings = "";
}