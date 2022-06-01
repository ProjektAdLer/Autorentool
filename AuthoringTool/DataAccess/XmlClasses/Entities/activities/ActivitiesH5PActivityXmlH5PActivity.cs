using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="h5pactivity")]
public class ActivitiesH5PActivityXmlH5PActivity : IActivitiesH5PActivityXmlH5PActivity{
    
    public void SetParameterts(string? name, string? timecreated, string? timemodified, string? intro, string? introformat, 
        string? grade, string? displayoptions, string? enabletracking, string? grademethod, string? reviewmode, 
        string? attempts, string? id)
    {
        Name = name;
        Timecreated = timecreated;
        Timemodified = timemodified;
        Intro = intro;
        Introformat = introformat;
        Grade = grade;
        Displayoptions = displayoptions;
        Enabletracking = enabletracking;
        Grademethod = grademethod;
        Reviewmode = reviewmode;
        Attempts = attempts;
        Id = id;
    }

    
    [XmlElement(ElementName="name")]
    public string? Name { get; set; }
    
    [XmlElement(ElementName="timecreated")]
    public string? Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string? Timemodified { get; set; }
    
    [XmlElement(ElementName="intro")]
    public string? Intro { get; set; }
    
    [XmlElement(ElementName="introformat")]
    public string? Introformat { get; set; }
    
    [XmlElement(ElementName="grade")]
    public string? Grade { get; set; }
    
    [XmlElement(ElementName="displayoptions")]
    public string? Displayoptions { get; set; }
    
    [XmlElement(ElementName="enabletracking")]
    public string? Enabletracking { get; set; }
    
    [XmlElement(ElementName="grademethod")]
    public string? Grademethod { get; set; }
    
    [XmlElement(ElementName="reviewmode")]
    public string? Reviewmode { get; set; }
    
    [XmlElement(ElementName="attempts")]
    public string? Attempts { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
}