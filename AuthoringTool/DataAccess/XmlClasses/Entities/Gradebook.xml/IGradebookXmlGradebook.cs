namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IGradebookXmlGradebook : IXmlSerializable
{

    string Attributes { get; set; }
    GradebookXmlGradeCategories GradeCategories { get; set; }
    GradebookXmlGradeItems GradeItems { get; set; }
    string GradeLetters { get; set; }
    GradebookXmlGradeSettings GradeSettings { get; set; }
}