using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="grade_itemref")]
public class ActivitiesInforefXmlGradeItemref : IActivitiesInforefXmlGradeItemref{
    
    public void SetParameters(ActivitiesInforefXmlGradeItem? gradeItem)
    {
        Grade_item = gradeItem;
    }
    
    [XmlElement(ElementName="grade_item")]
    public ActivitiesInforefXmlGradeItem? Grade_item { get; set; }
}