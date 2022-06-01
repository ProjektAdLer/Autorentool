using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;


[XmlRoot(ElementName="grade_category")]
public class GradebookXmlGradeCategory : IGradebookXmlGradeCategory{
    
    public void SetParameters(string? parent, string? depth, string? path, string? fullname, string? aggregation, 
        string? keephigh, string? droplow, string? aggregateonlygraded, string? aggregateoutcomes, string? timecreated, 
        string? timemodified, string? hidden, string? id)
    {
        Parent = parent;
        Depth = depth;
        Path = path;
        Fullname = fullname;
        Aggregation = aggregation;
        Keephigh = keephigh;
        Droplow = droplow;
        Aggregateonlygraded = aggregateonlygraded;
        Aggregateoutcomes = aggregateoutcomes;
        Timecreated = timecreated;
        Timemodified = timemodified;
        Hidden = hidden;
        Id = id;
    }

    [XmlElement(ElementName="parent")]
    public string? Parent { get; set; }
    
    [XmlElement(ElementName="depth")]
    public string? Depth { get; set; }
    
    [XmlElement(ElementName="path")]
    public string? Path { get; set; }
    
    [XmlElement(ElementName="fullname")]
    public string? Fullname { get; set; }
    
    [XmlElement(ElementName="aggregation")]
    public string? Aggregation { get; set; }
    
    [XmlElement(ElementName="keephigh")]
    public string? Keephigh { get; set; }
    
    [XmlElement(ElementName="droplow")]
    public string? Droplow { get; set; }
    
    [XmlElement(ElementName="aggregateonlygraded")]
    public string? Aggregateonlygraded { get; set; }
    
    [XmlElement(ElementName="aggregateoutcomes")]
    public string? Aggregateoutcomes { get; set; }
    
    [XmlElement(ElementName="timecreated")]
    public string? Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string? Timemodified { get; set; }
    
    [XmlElement(ElementName="hidden")]
    public string? Hidden { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
    
}