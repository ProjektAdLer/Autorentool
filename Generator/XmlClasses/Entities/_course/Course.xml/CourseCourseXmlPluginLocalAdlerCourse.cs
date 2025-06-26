using System.Xml.Serialization;
using Shared;

namespace Generator.XmlClasses.Entities._course.Course.xml;

public class CourseCourseXmlPluginLocalAdlerCourse : ICourseCourseXmlPluginLocalAdlerCourse
{
    public CourseCourseXmlPluginLocalAdlerCourse()
    {
        AdlerCourse = new CourseCourseXmlAdlerCourse();
    }

    [XmlElement(ElementName = "plugin_release_set_version")]
    public string PluginReleaseSetVersion { get; set; } = Constants.PluginReleaseSetVersion;

    [XmlElement(ElementName = "adler_course")]
    public CourseCourseXmlAdlerCourse AdlerCourse { get; set; }
}