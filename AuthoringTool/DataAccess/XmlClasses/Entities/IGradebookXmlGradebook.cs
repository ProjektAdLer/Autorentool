namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IGradebookXmlGradebook : IXmlSerializable
{
    void SetParameters(string? attributes, GradebookXmlGradeCategories? gradeCategories,
        GradebookXmlGradeItems? gradeItems, string? gradeLetters, GradebookXmlGradeSettings? gradeSettings);
}