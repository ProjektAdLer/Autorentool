using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;

[XmlRoot(ElementName = "file")]
public class FilesXmlFile : IFilesXmlFile, ICloneable
{

    public FilesXmlFile()
    {
        Id = "";
        ContentHash = "";
        ContextId = "";
        Component = "mod_resource";
        FileArea = "content";
        ItemId = "0";
        Filename = "";
        Filepath = "/";
        Filesize = "";
        Mimetype = "application/json";
        Timecreated = "";
        Timemodified = ""; 
        Source = "";
        Author = "$@NULL@$";
        Sortorder = "0";
        Userid = "$@NULL@$";
        Status = "0";
        License = "unknown";
        RepositoryType = "$@NULL@$";
        RepositoryId = "$@NULL@$";
        Reference = "$@NULL@$";
    }
    
    [XmlElement(ElementName="contenthash")]
    public string ContentHash { get; set; }
    
    [XmlElement(ElementName="contextid")]
    public string ContextId { get; set; }
    
    [XmlElement(ElementName="component")]
    public string Component { get; set; }
    
    [XmlElement(ElementName="filearea")]
    public string FileArea { get; set; }
    
    [XmlElement(ElementName="itemid")]
    public string ItemId { get; set; }
    
    [XmlElement(ElementName="filepath")]
    public string Filepath { get; set; }
    
    [XmlElement(ElementName="filename")]
    public string Filename { get; set; }
    
    [XmlElement(ElementName="userid")]
    public string Userid { get; set; }
    
    [XmlElement(ElementName="filesize")]
    public string Filesize { get; set; }
    
    [XmlElement(ElementName="mimetype")]
    public string Mimetype { get; set; }
    
    [XmlElement(ElementName="status")]
    public string Status { get; set; }
    
    [XmlElement(ElementName="timecreated")]
    public string Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string Timemodified { get; set; }
    
    //Has the same Value as Filename
    [XmlElement(ElementName="source")]
    public string Source { get; set; }
    
    [XmlElement(ElementName="author")]
    public string Author { get; set; }
    
    [XmlElement(ElementName="license")]
    public string License { get; set; }
    
    [XmlElement(ElementName="sortorder")]
    public string Sortorder { get; set; }

    [XmlElement(ElementName = "repositorytype")]
    public string RepositoryType { get; set; }

    [XmlElement(ElementName = "repositoryid")]
    public string RepositoryId { get; set; }

    [XmlElement(ElementName = "reference")]
    public string Reference { get; set; }
    
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}