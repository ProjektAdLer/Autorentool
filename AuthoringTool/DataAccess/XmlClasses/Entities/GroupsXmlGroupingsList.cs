using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="GroupingsList")]
public partial class GroupsXmlGroupingsList
{
    public void SetParameters(string groupings)
    {
        Groupings = groupings;
    }

    [XmlElement(ElementName = "groupings")]
    public string Groupings = "";
}