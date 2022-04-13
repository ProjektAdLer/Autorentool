namespace AuthoringTool.DataAccess.XmlClasses;

public interface IGradebookXmlGradebook : IXmlSerializable
{
    void SetParameters(GradebookXmlGradeSettings? gradeSettings);
}