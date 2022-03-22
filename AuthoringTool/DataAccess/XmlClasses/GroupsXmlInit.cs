using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

public class GroupsXmlInit : IXMLInit
{
        public void XmlInit()
        {
            var group = new GroupsXmlGroups();
            var groupingsList = new GroupsXmlGroupingsList();

            group.GroupingsList = groupingsList;
            
            var xml = new XmlSer();
            xml.serialize(group, "groups.xml");
            
        } 
}