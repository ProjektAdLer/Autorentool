using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

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
        xml.Serialize(this, "course/enrolments.xml");
    }
    
    [XmlElement(ElementName = "enrols")] 
    public CourseEnrolmentsXmlEnrols Enrols { get; set; }
}