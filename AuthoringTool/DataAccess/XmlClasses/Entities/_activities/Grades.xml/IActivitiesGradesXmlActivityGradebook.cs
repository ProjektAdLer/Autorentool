namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesGradesXmlActivityGradebook : IXmlSerializablePath
{
    ActivitiesGradesXmlGradeItems GradeItems { get; set; }

    string GradeLetters { get; set; }
}