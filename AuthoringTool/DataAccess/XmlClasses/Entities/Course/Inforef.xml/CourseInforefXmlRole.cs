using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Inforef.xml;

[XmlRoot(ElementName="role")]
public class CourseInforefXmlRole : ICourseInforefXmlRole{

    public CourseInforefXmlRole()
    {
        Id = "5";
    }
    
    [XmlElement(ElementName="id")]
    public string Id { get; set; }
}