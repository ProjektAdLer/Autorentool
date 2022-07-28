using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="grade_items")]
public class GradebookXmlGradeItems : IGradebookXmlGradeItems {

    public GradebookXmlGradeItems()
    {
        GradeItem = new GradebookXmlGradeItem();
    }
    
    [XmlElement(ElementName="grade_item")]
    public GradebookXmlGradeItem GradeItem { get; set; }
    
}