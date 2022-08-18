namespace Generator.XmlClasses.Entities._activities.Grades.xml;

public interface IActivitiesGradesXmlActivityGradebook : IXmlSerializablePath
{
    ActivitiesGradesXmlGradeItems GradeItems { get; set; }

    string GradeLetters { get; set; }
}