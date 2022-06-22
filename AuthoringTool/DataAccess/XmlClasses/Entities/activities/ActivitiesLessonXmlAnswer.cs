using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="answer")]
public class ActivitiesLessonXmlAnswer : IActivitiesLessonXmlAnswer {
    
    public void SetParameters(string? jumpto, string? grade, string? score, string? flags, string? timecreated, 
        string? timemodified, string? answerText, string? response, string? answerformat, string? responseformat, 
        string? attempts, string? id)
    {
        Jumpto = jumpto;
        Grade = grade;
        Score = score;
        Flags = flags;
        Timecreated = timecreated;
        Timemodified = timemodified;
        Answer_text = answerText;
        Response = response;
        Answerformat = answerformat;
        Responseformat = responseformat;
        Attempts = attempts;
        Id = id;
    }

    [XmlElement(ElementName="jumpto")]
    public string? Jumpto { get; set; }
        
    [XmlElement(ElementName="grade")]
    public string? Grade { get; set; }
        
    [XmlElement(ElementName="score")]
    public string? Score { get; set; }
        
    [XmlElement(ElementName="flags")]
    public string? Flags { get; set; }
        
    [XmlElement(ElementName="timecreated")]
    public string? Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string? Timemodified { get; set; }
        
    [XmlElement(ElementName="answer_text")]
    public string? Answer_text { get; set; }
        
    [XmlElement(ElementName="response")]
    public string? Response { get; set; }
        
    [XmlElement(ElementName="answerformat")]
    public string? Answerformat { get; set; }
        
    [XmlElement(ElementName="responseformat")]
    public string? Responseformat { get; set; }
    
    [XmlElement(ElementName="attempts")]
    public string? Attempts { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
    
}
