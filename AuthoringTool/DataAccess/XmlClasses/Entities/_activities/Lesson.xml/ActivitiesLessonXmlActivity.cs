using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities._activities.Lesson.xml;


[XmlRoot(ElementName="activity")]
public class ActivitiesLessonXmlActivity :IActivitiesLessonXmlActivity {

    public ActivitiesLessonXmlActivity()
    {
        Lesson = new ActivitiesLessonXmlLesson();
        Id = "";
        ModuleId = "";
        ModuleName = "lesson";
        ContextId = "1";
    }
    
    public void Serialize(string activityName, string moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "lesson.xml"));
    }

    [XmlElement(ElementName="lesson")]
    public ActivitiesLessonXmlLesson Lesson { get; set; }
        
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
        
    [XmlAttribute(AttributeName="moduleid")]
    public string ModuleId { get; set; }
        
    [XmlAttribute(AttributeName="modulename")]
    public string ModuleName { get; set; }
        
    [XmlAttribute(AttributeName="contextid")]
    public string ContextId { get; set; }
        
}