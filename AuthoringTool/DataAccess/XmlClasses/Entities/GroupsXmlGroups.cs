﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="groups")]
public partial class GroupsXmlGroups : IXmlSerializable{

    public GroupsXmlGroups(GroupsXmlGroupingsList groupingsList)
    {
        GroupingsList = groupingsList;
    }
    
    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "groups.xml");
    }
    
    [XmlElement(ElementName="GroupingsList")]
    public GroupsXmlGroupingsList GroupingsList { get; set; }
}