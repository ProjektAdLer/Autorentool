using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._course.Inforef.xml;

[XmlRoot(ElementName="inforef")]
public class CourseInforefXmlInforef : ICourseInforefXmlInforef {

    public CourseInforefXmlInforef()
    {
        Roleref = new CourseInforefXmlRoleref();
    }


    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("course", "inforef.xml")); 
    }
    
    [XmlElement(ElementName="roleref")]
    public CourseInforefXmlRoleref Roleref { get; set; }
}