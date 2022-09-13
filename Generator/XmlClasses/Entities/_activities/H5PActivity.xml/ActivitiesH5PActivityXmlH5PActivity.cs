using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.H5PActivity.xml;


[XmlRoot(ElementName="h5pactivity")]
public class ActivitiesH5PActivityXmlH5PActivity : IActivitiesH5PActivityXmlH5PActivity{

    public ActivitiesH5PActivityXmlH5PActivity()
    {
        Name = "";
        Timecreated = "";
        Timemodified = "";
        Intro = "";
        Introformat = "1";
        Grade = "100";
        DisplayOptions = "15";
        Enabletracking = "1";
        Grademethod = "1";
        Reviewmode = "1";
        Attempts = "";
        Id = "";
    }
    
    [XmlElement(ElementName="name")]
    public string Name { get; set; }
    
    [XmlElement(ElementName="timecreated")]
    public string Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string Timemodified { get; set; }
    
    [XmlElement(ElementName="intro")]
    public string Intro { get; set; }
    
    [XmlElement(ElementName="introformat")]
    public string Introformat { get; set; }
    
    [XmlElement(ElementName="grade")]
    public string Grade { get; set; }
    
    [XmlElement(ElementName="displayoptions")]
    public string DisplayOptions { get; set; }
    
    [XmlElement(ElementName="enabletracking")]
    public string Enabletracking { get; set; }
    
    [XmlElement(ElementName="grademethod")]
    public string Grademethod { get; set; }
    
    [XmlElement(ElementName="reviewmode")]
    public string Reviewmode { get; set; }
    
    [XmlElement(ElementName="attempts")]
    public string Attempts { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
}