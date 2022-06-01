using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="module")]
public class ActivitiesModuleXmlModule : IActivitiesModuleXmlModule{
    
    public void SetParameterts(string? modulename, string? sectionid, string? sectionnumber, string? idnumber, 
        string? added, string? score, string? indent, string? visible, string? visibleoncoursepage, string? visibleold, 
        string? groupmode, string? groupingid, string? completion, string? completiongradeitemnumber, string? completionview, 
        string? completionexpected, string? availability, string? showdescription, string? tags, string? id, string? version)
    {
        Modulename = modulename;
        Sectionid = sectionid;
        Sectionnumber = sectionnumber;
        Idnumber = idnumber;
        Added = added;
        Score = score;
        Indent = indent;
        Visible = visible;
        Visibleoncoursepage = visibleoncoursepage;
        Visibleold = visibleold;
        Groupmode = groupmode;
        Groupingid = groupingid;
        Completion = completion;
        Completiongradeitemnumber = completiongradeitemnumber;
        Completionview = completionview;
        Completionexpected = completionexpected;
        Availability = availability;
        Showdescription = showdescription;
        Tags = tags;
        Id = id;
        Version = version;
    }
    
    public void Serialize(string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", "h5pactivity_"+moduleId, "module.xml"));
    }

    
    [XmlElement(ElementName="modulename")]
    public string? Modulename { get; set; }
    
    [XmlElement(ElementName="sectionid")]
    public string? Sectionid { get; set; }
    
    [XmlElement(ElementName="sectionnumber")]
    public string? Sectionnumber { get; set; }
    
    [XmlElement(ElementName="idnumber")]
    public string? Idnumber { get; set; }
    
    [XmlElement(ElementName="added")]
    public string? Added { get; set; }
    
    [XmlElement(ElementName="score")]
    public string? Score { get; set; }
    
    [XmlElement(ElementName="indent")]
    public string? Indent { get; set; }
    
    [XmlElement(ElementName="visible")]
    public string? Visible { get; set; }
    
    [XmlElement(ElementName="visibleoncoursepage")]
    public string? Visibleoncoursepage { get; set; }
    
    [XmlElement(ElementName="visibleold")]
    public string? Visibleold { get; set; }
    
    [XmlElement(ElementName="groupmode")]
    public string? Groupmode { get; set; }
    
    [XmlElement(ElementName="groupingid")]
    public string? Groupingid { get; set; }
    
    [XmlElement(ElementName="completion")]
    public string? Completion { get; set; }
    
    [XmlElement(ElementName="completiongradeitemnumber")]
    public string? Completiongradeitemnumber { get; set; }
    
    [XmlElement(ElementName="completionview")]
    public string? Completionview { get; set; }
    
    [XmlElement(ElementName="completionexpected")]
    public string? Completionexpected { get; set; }
    
    [XmlElement(ElementName="availability")]
    public string? Availability { get; set; }
    
    [XmlElement(ElementName="showdescription")]
    public string? Showdescription { get; set; }
    
    [XmlElement(ElementName="tags")]
    public string? Tags { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
    
    [XmlAttribute(AttributeName="version")]
    public string? Version { get; set; }
    
}