using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;

public class CourseInforefXmlInit : IXMLInit
{
    public void XmlInit()
    {
            var courseInforef = new CourseInforefXmlInforef();
            var courseRoleref = new CourseInforefXmlRoleref();
            var courseInforefRole = new CourseInforefXmlRole();

            courseInforefRole.Id = "5";
            courseRoleref.Role = courseInforefRole;
            courseInforef.Roleref = courseRoleref;
            
            var xml = new XmlSer();
            xml.serialize(courseInforef, "course/inforef.xml"); 

    }
}