using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="answers")] 
public class ActivitiesLessonXmlAnswers : IActivitiesLessonXmlAnswers{
    
    public void SetParameters(ActivitiesLessonXmlAnswer? answer)
    {
        Answer = answer;
    }

    [XmlElement(ElementName="answer")]
    public ActivitiesLessonXmlAnswer? Answer { get; set; }
    
}