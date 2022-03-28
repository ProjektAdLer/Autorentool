﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="roles")]
public partial class CourseRolesXmlRoles : IXmlSerializable
{
    public void SetParameters(string roleOverrides, string roleAssignments)
    {
        Role_overrides = roleOverrides;
        Role_assignments = Role_assignments;
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "course/roles.xml");
    }


    [XmlElement(ElementName = "role_overrides")]
    public string Role_overrides = "";

    [XmlElement(ElementName = "role_assignments")]
    public string Role_assignments = "";
}