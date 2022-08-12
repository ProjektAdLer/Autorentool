using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Enrolments.xml;

[XmlRoot(ElementName="enrols")]
public class CourseEnrolmentsXmlEnrols : ICourseEnrolmentsXmlEnrols{

    public CourseEnrolmentsXmlEnrols()
    {
        Enrol = new List<CourseEnrolmentsXmlEnrol>();
    }
    
    [XmlElement(ElementName="enrol")] 
    public List<CourseEnrolmentsXmlEnrol> Enrol { get; set; }
}