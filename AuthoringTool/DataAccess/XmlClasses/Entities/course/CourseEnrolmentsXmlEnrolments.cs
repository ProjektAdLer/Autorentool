using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

[XmlRoot(ElementName="enrolments")]
public partial class CourseEnrolmentsXmlEnrolments : ICourseEnrolmentsXmlEnrolments
{
    
    public void SetParameters(CourseEnrolmentsXmlEnrols? enrolsList)
    {
        Enrols = enrolsList;
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "course/enrolments.xml");
    }
    
    [XmlElement(ElementName = "enrols")] 
    public CourseEnrolmentsXmlEnrols? Enrols;
}