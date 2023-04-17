using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._activities.Module.xml;


[XmlRoot(ElementName="module")]
public class ActivitiesModuleXmlModule : IActivitiesModuleXmlModule{

    public ActivitiesModuleXmlModule()
    {
        ModuleName = "";
        SectionId = "";
        SectionNumber = "";
        IdNumber = "";
        Added = "";
        Score = "0";
        Indent = "0";
        Visible = "1";
        VisibleOnCoursePage = "1";
        Visibleold = "1";
        Groupmode = "0";
        GroupingId = "1";
        Completion = "1";
        Completiongradeitemnumber = "$@NULL@$";
        CompletionView = "0";
        Completionexpected = "0";
        Availability = "$@NULL@$";
        ShowDescription = "0";
        PluginLocalAdlerModule = new ActivitiesModuleXmlPluginLocalAdlerModule();
        Tags = "";
        Id = "";
        Version = "2021051700";
    }

    
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "module.xml"));
    }

    
    [XmlElement(ElementName="modulename")]
    public string ModuleName { get; set; }
    
    [XmlElement(ElementName="sectionid")]
    public string SectionId { get; set; }
    
    [XmlElement(ElementName="sectionnumber")]
    public string SectionNumber { get; set; }
    
    [XmlElement(ElementName="idnumber")]
    public string IdNumber { get; set; }
    
    [XmlElement(ElementName="added")]
    public string Added { get; set; }
    
    [XmlElement(ElementName="score")]
    public string Score { get; set; }
    
    [XmlElement(ElementName="indent")]
    public string Indent { get; set; }
    
    [XmlElement(ElementName="visible")]
    public string Visible { get; set; }
    
    [XmlElement(ElementName="visibleoncoursepage")]
    public string VisibleOnCoursePage { get; set; }
    
    [XmlElement(ElementName="visibleold")]
    public string Visibleold { get; set; }
    
    [XmlElement(ElementName="groupmode")]
    public string Groupmode { get; set; }
    
    [XmlElement(ElementName="groupingid")]
    public string GroupingId { get; set; }
    
    [XmlElement(ElementName="completion")]
    public string Completion { get; set; }
    
    [XmlElement(ElementName="completiongradeitemnumber")]
    public string Completiongradeitemnumber { get; set; }
    
    [XmlElement(ElementName="completionview")]
    public string CompletionView { get; set; }
    
    [XmlElement(ElementName="completionexpected")]
    public string Completionexpected { get; set; }
    
    [XmlElement(ElementName="availability")]
    public string Availability { get; set; }
    
    [XmlElement(ElementName="showdescription")]
    public string ShowDescription { get; set; }
    
    [XmlElement(ElementName = "plugin_local_adler_module")]
    public ActivitiesModuleXmlPluginLocalAdlerModule PluginLocalAdlerModule { get; set; }

    [XmlElement(ElementName="tags")]
    public string Tags { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
    
    [XmlAttribute(AttributeName="version")]
    public string Version { get; set; }
    
}