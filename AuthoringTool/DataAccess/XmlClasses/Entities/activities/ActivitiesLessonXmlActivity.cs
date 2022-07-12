using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="activity")]
public class ActivitiesLessonXmlActivity :IActivitiesLessonXmlActivity {
    
    public void SetParameters(ActivitiesLessonXmlLesson? lesson, string? id, string? moduleid, string? modulename, string? contextid)
    {
        Lesson = lesson;
        Id = id;
        Moduleid = moduleid;
        Modulename = modulename;
        Contextid = contextid;
    }
    
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "lesson.xml"));
    }

    [XmlElement(ElementName="lesson")]
    public ActivitiesLessonXmlLesson? Lesson { get; set; }
        
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
        
    [XmlAttribute(AttributeName="moduleid")]
    public string? Moduleid { get; set; }
        
    [XmlAttribute(AttributeName="modulename")]
    public string? Modulename { get; set; }
        
    [XmlAttribute(AttributeName="contextid")]
    public string? Contextid { get; set; }
        
}