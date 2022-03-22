using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course

{
    [XmlRoot(ElementName="roles")]
    public class CourseRolesXmlRoles
    {
        [XmlElement(ElementName = "role_overrides")]
        public string Role_overrides = "";

        [XmlElement(ElementName = "role_assignments")]
        public string Role_assignments = "";
    }

    public class CourseRolesXmlInit
    {
        public CourseRolesXmlRoles Init()
        {
            var courseRole = new CourseRolesXmlRoles();
            
            var xml = new XmlSer();
            xml.serialize(courseRole, "course/roles.xml");
            
            return courseRole;
        }
    }

}