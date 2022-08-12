using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Inforef.xml;

[XmlRoot(ElementName="roleref")]
public class CourseInforefXmlRoleref : ICourseInforefXmlRoleref{

    public CourseInforefXmlRoleref()
    {
        Role = new CourseInforefXmlRole();
    }
    
    
    [XmlElement(ElementName="role")]
    public CourseInforefXmlRole Role { get; set; }
}