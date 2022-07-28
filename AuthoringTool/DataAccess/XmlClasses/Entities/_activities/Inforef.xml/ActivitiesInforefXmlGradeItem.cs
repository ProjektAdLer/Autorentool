using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

[XmlRoot(ElementName="grade_item")]
public class ActivitiesInforefXmlGradeItem : IActivitiesInforefXmlGradeItem{

    public ActivitiesInforefXmlGradeItem()
    {
        Id = "1";
    }
    
   
    [XmlElement(ElementName="id")]
    public string Id { get; set; }
}