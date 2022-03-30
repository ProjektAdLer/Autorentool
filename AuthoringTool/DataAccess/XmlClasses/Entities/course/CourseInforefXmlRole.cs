using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="role")]
public partial class CourseInforefXmlRole {

    
    public void SetParameters(string id)
    {
        Id = id;
    }
    
    [XmlElement(ElementName="id")]
    public string Id = "";
}