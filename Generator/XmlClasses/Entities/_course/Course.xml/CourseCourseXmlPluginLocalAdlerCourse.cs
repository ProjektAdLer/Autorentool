using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._course.Course.xml;

public class CourseCourseXmlPluginLocalAdlerCourse : ICourseCourseXmlPluginLocalAdlerCourse
{
    
    public CourseCourseXmlPluginLocalAdlerCourse()
    {
        AdlerCourse = new CourseCourseXmlAdlerCourse();
    }
    
    [XmlElement(ElementName = "adler_course")]
    public CourseCourseXmlAdlerCourse AdlerCourse { get; set; }
}