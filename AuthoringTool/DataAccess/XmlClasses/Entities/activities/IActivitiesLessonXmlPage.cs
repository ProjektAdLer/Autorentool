namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesLessonXmlPage
{
    void SetParameters(string? prevpageid, string? nextpageid, string? qtype, string? qoption,
        string? layout, string? display, string? timecreated, string? timemodified, string? title,
        string? contents, string? contentsformat, ActivitiesLessonXmlAnswers? answers, string? branches, string? id);
}