namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesH5PActivityXmlH5PActivity
{
    void SetParameterts(string? name, string? timecreated, string? timemodified, string? intro, string? introformat,
        string? grade, string? displayoptions, string? enabletracking, string? grademethod, string? reviewmode,
        string? attempts, string? id);
}