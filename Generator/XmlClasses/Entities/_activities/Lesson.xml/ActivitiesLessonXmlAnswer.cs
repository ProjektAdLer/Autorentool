using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities._activities.Lesson.xml;


[XmlRoot(ElementName="answer")]
public class ActivitiesLessonXmlAnswer : IActivitiesLessonXmlAnswer {

    public ActivitiesLessonXmlAnswer()
    {
        JumpTo = "0";
        Grade = "0";
        Score = "0";
        Flags = "0";
        Timecreated = "";
        Timemodified = "";
        AnswerText = "";
        Response = "$@NULL@$";
        Answerformat = "0";
        Responseformat = "0";
        Attempts = "";
        Id = "";
    }
    

    [XmlElement(ElementName="jumpto")]
    public string JumpTo { get; set; }
        
    [XmlElement(ElementName="grade")]
    public string Grade { get; set; }
        
    [XmlElement(ElementName="score")]
    public string Score { get; set; }
        
    [XmlElement(ElementName="flags")]
    public string Flags { get; set; }
        
    [XmlElement(ElementName="timecreated")]
    public string Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string Timemodified { get; set; }
        
    [XmlElement(ElementName="answer_text")]
    public string AnswerText { get; set; }
        
    [XmlElement(ElementName="response")]
    public string Response { get; set; }
        
    [XmlElement(ElementName="answerformat")]
    public string Answerformat { get; set; }
        
    [XmlElement(ElementName="responseformat")]
    public string Responseformat { get; set; }
    
    [XmlElement(ElementName="attempts")]
    public string Attempts { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
    
}
