using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Completiondefault.xml;

[XmlRoot(ElementName="course_completion_defaults")]
public class CourseCompletiondefaultXmlCourseCompletionDefaults : ICourseCompletiondefaultXmlCourseCompletionDefaults{
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "course/completiondefaults.xml");
    }
}