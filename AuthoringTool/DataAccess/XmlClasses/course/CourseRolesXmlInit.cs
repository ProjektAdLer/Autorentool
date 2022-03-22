using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;
public class CourseRolesXmlInit : IXMLInit
{
    public void XmlInit()
    {
            var courseRole = new CourseRolesXmlRoles();
            
            var xml = new XmlSer();
            xml.serialize(courseRole, "course/roles.xml");

    }
}