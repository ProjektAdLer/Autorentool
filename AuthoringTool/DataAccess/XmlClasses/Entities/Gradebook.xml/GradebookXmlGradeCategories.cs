using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Gradebook.xml;


[XmlRoot(ElementName="grade_categories")]
public class GradebookXmlGradeCategories : IGradebookXmlGradeCategories {

    public GradebookXmlGradeCategories()
    {
        GradeCategory = new GradebookXmlGradeCategory();
    }
    

    [XmlElement(ElementName="grade_category")]
    public GradebookXmlGradeCategory GradeCategory { get; set; }
    
}