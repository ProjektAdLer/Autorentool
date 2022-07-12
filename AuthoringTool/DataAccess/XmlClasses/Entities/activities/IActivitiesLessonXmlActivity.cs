namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesLessonXmlActivity : IXmlSerializablePath
{

    void SetParameters(ActivitiesLessonXmlLesson? lesson, string? id, string? moduleid, string? modulename,
        string? contextid);
    
    void Serialize(string? activityName, string? moduleId);
}