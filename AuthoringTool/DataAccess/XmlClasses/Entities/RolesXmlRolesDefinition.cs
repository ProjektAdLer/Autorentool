﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="roles_definition")]
public partial class RolesXmlRolesDefinition : IXmlSerializable {

    public RolesXmlRolesDefinition(RolesXmlRole rolesRole)
    {
        Role = rolesRole;
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "roles.xml");
    }
    
    [XmlElement(ElementName="role")]
    public RolesXmlRole Role { get; set; }
}