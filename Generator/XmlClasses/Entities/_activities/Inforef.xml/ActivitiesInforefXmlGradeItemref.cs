using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Inforef.xml;


[XmlRoot(ElementName="grade_itemref")]
public class ActivitiesInforefXmlGradeItemref : IActivitiesInforefXmlGradeItemref{

    public ActivitiesInforefXmlGradeItemref()
    {
        GradeItem = new ActivitiesInforefXmlGradeItem();
    }
    
    [XmlElement(ElementName="grade_item")]
    public ActivitiesInforefXmlGradeItem GradeItem { get; set; }
}