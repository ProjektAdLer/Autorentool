using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="enrols")]
public partial class CourseEnrolmentsXmlEnrols {
    [XmlElement(ElementName="enrol")] 
    public List<CourseEnrolmentsXmlEnrol> Enrol;
}