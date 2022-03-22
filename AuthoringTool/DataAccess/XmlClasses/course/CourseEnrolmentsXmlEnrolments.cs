using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="enrolments")]
public partial class CourseEnrolmentsXmlEnrolments
{
    [XmlElement(ElementName = "enrols")] 
    public CourseEnrolmentsXmlEnrols Enrols;
}