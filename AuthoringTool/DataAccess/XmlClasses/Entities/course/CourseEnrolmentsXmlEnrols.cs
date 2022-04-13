using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="enrols")]
public partial class CourseEnrolmentsXmlEnrols : ICourseEnrolmentsXmlEnrols{
    
    
    public void SetParameters(CourseEnrolmentsXmlEnrol? enrol1,
                                      CourseEnrolmentsXmlEnrol? enrol2, CourseEnrolmentsXmlEnrol? enrol3)
    {
        Enrol = new List<CourseEnrolmentsXmlEnrol?>();
        Enrol.Add(enrol1);
        Enrol.Add(enrol2);
        Enrol.Add(enrol3);
    }
    
    [XmlElement(ElementName="enrol")] 
    public List<CourseEnrolmentsXmlEnrol?> Enrol;
}