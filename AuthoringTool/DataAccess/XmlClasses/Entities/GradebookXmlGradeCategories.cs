using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;


[XmlRoot(ElementName="grade_categories")]
public class GradebookXmlGradeCategories : IGradebookXmlGradeCategories {
    
    public void SetParameters(GradebookXmlGradeCategory? gradeCategory)
    {
        Grade_category = gradeCategory;
    }

    [XmlElement(ElementName="grade_category")]
    public GradebookXmlGradeCategory? Grade_category { get; set; }
    
}