using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="enrolments")]
public partial class CourseEnrolmentsXmlEnrolments : IXmlSerializable
{
    public CourseEnrolmentsXmlEnrolments(CourseEnrolmentsXmlEnrols enrolsList)
    {
        Enrols = enrolsList;
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "course/enrolments.xml");
    }
    
    [XmlElement(ElementName = "enrols")] 
    public CourseEnrolmentsXmlEnrols Enrols;
}