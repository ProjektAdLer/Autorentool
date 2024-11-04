using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._course.Course.xml;

public class CourseCourseXmlAdlerCourse : ICourseCourseXmlAdlerCourse
{
    public CourseCourseXmlAdlerCourse()
    {
        Uuid = "";
    }

    [XmlElement(ElementName = "uuid")] public string Uuid { get; set; }
}