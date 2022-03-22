using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="roleref")]
public partial class CourseInforefXmlRoleref {
    [XmlElement(ElementName="role")]
    public CourseInforefXmlRole Role;
}