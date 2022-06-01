using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="grade_items")]
public class GradebookXmlGradeItems : IGradebookXmlGradeItems {

    public void SetParameters(GradebookXmlGradeItem? gradeItem)
    {
        Grade_item = gradeItem;
    }
    
    [XmlElement(ElementName="grade_item")]
    public GradebookXmlGradeItem? Grade_item { get; set; }
    
}