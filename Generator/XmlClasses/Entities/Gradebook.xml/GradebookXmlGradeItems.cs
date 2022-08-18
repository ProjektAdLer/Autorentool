using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.Gradebook.xml;

[XmlRoot(ElementName="grade_items")]
public class GradebookXmlGradeItems : IGradebookXmlGradeItems {

    public GradebookXmlGradeItems()
    {
        GradeItem = new GradebookXmlGradeItem();
    }
    
    [XmlElement(ElementName="grade_item")]
    public GradebookXmlGradeItem GradeItem { get; set; }
    
}