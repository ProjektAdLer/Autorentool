using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="role")]
public partial class CourseInforefXmlRole {

    public CourseInforefXmlRole(string id)
    {
        Id = id;
    }
    
    [XmlElement(ElementName="id")]
    public string Id = "";
}