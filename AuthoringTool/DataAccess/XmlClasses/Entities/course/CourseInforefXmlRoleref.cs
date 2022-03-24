using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="roleref")]
public partial class CourseInforefXmlRoleref {

    public CourseInforefXmlRoleref(CourseInforefXmlRole inforefRole)
    {
        Role = inforefRole;
    }
    
    [XmlElement(ElementName="role")]
    public CourseInforefXmlRole Role;
}