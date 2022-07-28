using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

[XmlRoot(ElementName="pages")]
public class ActivitiesLessonXmlPages : IActivitiesLessonXmlPages{

    public ActivitiesLessonXmlPages()
    {
        Page = new ActivitiesLessonXmlPage();
    }


    [XmlElement(ElementName="page")]
    public ActivitiesLessonXmlPage Page { get; set; }
    
}
    