using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="inforef")]
public partial class CourseInforefXmlInforef {
    [XmlElement(ElementName="roleref")]
    public CourseInforefXmlRoleref Roleref;
}