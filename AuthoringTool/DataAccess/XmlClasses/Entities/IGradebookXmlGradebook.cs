using AuthoringTool.DataAccess.XmlClasses.Entities;

namespace AuthoringTool.DataAccess.XmlClasses;

public interface IGradebookXmlGradebook : IXmlSerializable
{
    void SetParameters(string? attributes, GradebookXmlGradeCategories? gradeCategories,
        GradebookXmlGradeItems? gradeItems, string? gradeLetters, GradebookXmlGradeSettings? gradeSettings);
}