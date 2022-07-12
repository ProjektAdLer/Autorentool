using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

[XmlRoot(ElementName="pages")]
public class ActivitiesLessonXmlPages : IActivitiesLessonXmlPages{
    
    public void SetParameters(ActivitiesLessonXmlPage? page)
    {
        Page = page;
    }

    [XmlElement(ElementName="page")]
    public ActivitiesLessonXmlPage? Page { get; set; }
    
}
    