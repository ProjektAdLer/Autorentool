using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="role")]
public partial class CourseInforefXmlRole {
    [XmlElement(ElementName="id")]
    public string Id = "";
}