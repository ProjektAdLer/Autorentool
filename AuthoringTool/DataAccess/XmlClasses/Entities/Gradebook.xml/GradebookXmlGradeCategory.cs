using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;


[XmlRoot(ElementName="grade_category")]
public class GradebookXmlGradeCategory : IGradebookXmlGradeCategory{

    public GradebookXmlGradeCategory()
    {
        Parent = "$@NULL@$";
        Depth = "1";
        Path = "/1/";
        Fullname = "?";
        Aggregation = "13";
        Keephigh = "0";
        Droplow = "0";
        AggregateOnlyGraded = "1";
        AggregateOutcomes = "0";
        Timecreated = "";
        Timemodified = "";
        Hidden = "0";
        Id = "1";
    }
    

    [XmlElement(ElementName="parent")]
    public string Parent { get; set; }
    
    [XmlElement(ElementName="depth")]
    public string Depth { get; set; }
    
    [XmlElement(ElementName="path")]
    public string Path { get; set; }
    
    [XmlElement(ElementName="fullname")]
    public string Fullname { get; set; }
    
    [XmlElement(ElementName="aggregation")]
    public string Aggregation { get; set; }
    
    [XmlElement(ElementName="keephigh")]
    public string Keephigh { get; set; }
    
    [XmlElement(ElementName="droplow")]
    public string Droplow { get; set; }
    
    [XmlElement(ElementName="aggregateonlygraded")]
    public string AggregateOnlyGraded { get; set; }
    
    [XmlElement(ElementName="aggregateoutcomes")]
    public string AggregateOutcomes { get; set; }
    
    [XmlElement(ElementName="timecreated")]
    public string Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string Timemodified { get; set; }
    
    [XmlElement(ElementName="hidden")]
    public string Hidden { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
    
}