using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._course.Enrolments.xml;

[XmlRoot(ElementName="enrols")]
public class CourseEnrolmentsXmlEnrols : ICourseEnrolmentsXmlEnrols{

    public CourseEnrolmentsXmlEnrols()
    {
        Enrol = new List<CourseEnrolmentsXmlEnrol>();
    }
    
    [XmlElement(ElementName="enrol")] 
    public List<CourseEnrolmentsXmlEnrol> Enrol { get; set; }
}