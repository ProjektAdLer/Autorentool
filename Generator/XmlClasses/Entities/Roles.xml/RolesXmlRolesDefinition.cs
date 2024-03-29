﻿using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities.Roles.xml;

[XmlRoot(ElementName="roles_definition")]
public class RolesXmlRolesDefinition : IRolesXmlRolesDefinition {

    public RolesXmlRolesDefinition()
    {
        Role = new RolesXmlRole();
    }
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "roles.xml");
    }
    
    [XmlElement(ElementName="role")]
    public RolesXmlRole Role { get; set; }
}