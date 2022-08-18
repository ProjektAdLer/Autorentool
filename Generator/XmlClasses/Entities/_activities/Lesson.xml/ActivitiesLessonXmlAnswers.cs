using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Lesson.xml;


[XmlRoot(ElementName="answers")] 
public class ActivitiesLessonXmlAnswers : IActivitiesLessonXmlAnswers{

    public ActivitiesLessonXmlAnswers()
    {
        Answer = new ActivitiesLessonXmlAnswer();
    }
    

    [XmlElement(ElementName="answer")]
    public ActivitiesLessonXmlAnswer Answer { get; set; }
    
}