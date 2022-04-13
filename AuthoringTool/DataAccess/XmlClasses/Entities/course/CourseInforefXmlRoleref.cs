using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="roleref")]
public partial class CourseInforefXmlRoleref : ICourseInforefXmlRoleref{
    
    
    public void SetParameters(CourseInforefXmlRole? inforefRole)
    {
        Role = inforefRole;
    }
    
    [XmlElement(ElementName="role")]
    public CourseInforefXmlRole? Role;
}