using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._course.Completiondefault.xml;

[XmlRoot(ElementName="course_completion_defaults")]
public class CourseCompletiondefaultXmlCourseCompletionDefaults : ICourseCompletiondefaultXmlCourseCompletionDefaults{
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("course" ,"completiondefaults.xml"));
    }
}