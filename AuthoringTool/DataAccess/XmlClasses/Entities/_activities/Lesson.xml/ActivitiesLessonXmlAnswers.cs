using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="answers")] 
public class ActivitiesLessonXmlAnswers : IActivitiesLessonXmlAnswers{

    public ActivitiesLessonXmlAnswers()
    {
        Answer = new ActivitiesLessonXmlAnswer();
    }
    

    [XmlElement(ElementName="answer")]
    public ActivitiesLessonXmlAnswer Answer { get; set; }
    
}