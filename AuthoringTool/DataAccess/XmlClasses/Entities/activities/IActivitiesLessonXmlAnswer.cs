namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesLessonXmlAnswer
{
    void SetParameters(string? jumpto, string? grade, string? score, string? flags, string? timecreated,
        string? timemodified, string? answerText, string? response, string? answerformat, string? responseformat,
        string? attempts, string? id);
}