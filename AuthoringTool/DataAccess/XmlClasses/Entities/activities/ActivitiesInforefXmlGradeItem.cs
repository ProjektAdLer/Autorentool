using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

[XmlRoot(ElementName="grade_item")]
public class ActivitiesInforefXmlGradeItem : IActivitiesInforefXmlGradeItem{
    
    public void SetParameters(string? id)
    {
        Id = id;
    }
    
    [XmlElement(ElementName="id")]
    public string? Id { get; set; }
}