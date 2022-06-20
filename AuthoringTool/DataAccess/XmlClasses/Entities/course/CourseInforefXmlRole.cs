using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

[XmlRoot(ElementName="role")]
public partial class CourseInforefXmlRole : ICourseInforefXmlRole{

    
    public void SetParameters(string id)
    {
        Id = id;
    }
    
    [XmlElement(ElementName="id")]
    public string Id = "";
}