using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._course.Roles.xml;

[XmlRoot(ElementName="roles")]
public class CourseRolesXmlRoles : ICourseRolesXmlRoles
{

    public CourseRolesXmlRoles()
    {
        RoleOverrides = "";
        RoleAssignments = "";
    }
    

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "course/roles.xml");
    }


    [XmlElement(ElementName = "role_overrides")]
    public string RoleOverrides { get; set; }

    [XmlElement(ElementName = "role_assignments")]
    public string RoleAssignments { get; set; }
}