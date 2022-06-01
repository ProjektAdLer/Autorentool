namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesGradesXmlActivityGradebook : IXmlSerializablePath
{
    void SetParameterts(ActivitiesGradesXmlGradeItems? gradeItems, string? gradeLetters);
}