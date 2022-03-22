using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses

{
    [XmlRoot(ElementName="GroupingsList")]
    public class GroupsXmlGroupingsList
    {
        [XmlElement(ElementName = "groupings")]
        public string Groupings = "";
    }

    [XmlRoot(ElementName="groups")]
    public class GroupsXmlGroups {
        [XmlElement(ElementName="GroupingsList")]
        public GroupsXmlGroupingsList GroupingsList { get; set; }
    }

    public class GroupsXmlInit
    {
        public GroupsXmlGroups Init()
        {
            var group = new GroupsXmlGroups();
            var groupingsList = new GroupsXmlGroupingsList();

            group.GroupingsList = groupingsList;
            
            var xml = new XmlSer();
            xml.serialize(group, "groups.xml");
            
            return group;
        }
    }

}