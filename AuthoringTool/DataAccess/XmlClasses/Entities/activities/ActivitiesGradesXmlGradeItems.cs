using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="grade_items")]
public class ActivitiesGradesXmlGradeItems : IActivitiesGradesXmlGradeItems {
    
    public void SetParameters(ActivitiesGradesXmlGradeItem? gradeItem)
    {
        Grade_item = gradeItem;
    }
    
    [XmlElement(ElementName="grade_item")]
    public ActivitiesGradesXmlGradeItem? Grade_item { get; set; }
    
}