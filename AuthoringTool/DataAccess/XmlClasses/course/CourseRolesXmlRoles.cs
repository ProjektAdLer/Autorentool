using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="roles")]
public partial class CourseRolesXmlRoles
{
    [XmlElement(ElementName = "role_overrides")]
    public string Role_overrides = "";

    [XmlElement(ElementName = "role_assignments")]
    public string Role_assignments = "";
}