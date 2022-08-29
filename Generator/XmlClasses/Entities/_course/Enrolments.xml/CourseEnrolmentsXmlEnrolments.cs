using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._course.Enrolments.xml;

[XmlRoot(ElementName="enrolments")]
public class CourseEnrolmentsXmlEnrolments : ICourseEnrolmentsXmlEnrolments
{

    public CourseEnrolmentsXmlEnrolments()
    {
        Enrols = new CourseEnrolmentsXmlEnrols();
    }
    
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("course", "enrolments.xml"));
    }
    
    [XmlElement(ElementName = "enrols")] 
    public CourseEnrolmentsXmlEnrols Enrols { get; set; }
}