using System.Xml.Serialization;

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
            return group;
        }
    }

}