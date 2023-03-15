using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._course.Course.xml;

public class CourseCourseXmlAdlerCourse : ICourseCourseXmlAdlerCourse
{
    public CourseCourseXmlAdlerCourse()
    {
        Foo = "bar";
    }
    
    [XmlElement (ElementName = "foo")]
    public string Foo { get; set; }
}