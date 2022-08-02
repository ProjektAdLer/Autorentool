using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Inforef.xml;

[XmlRoot(ElementName="inforef")]
public class CourseInforefXmlInforef : ICourseInforefXmlInforef {

    public CourseInforefXmlInforef()
    {
        Roleref = new CourseInforefXmlRoleref();
    }


    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "course/inforef.xml"); 
    }
    
    [XmlElement(ElementName="roleref")]
    public CourseInforefXmlRoleref Roleref { get; set; }
}