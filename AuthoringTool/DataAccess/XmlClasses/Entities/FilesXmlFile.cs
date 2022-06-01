using System.Xml.Serialization;

using NLog.Targets;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName = "file")]
public partial class FilesXmlFile : IFilesXmlFile
{
    public void SetParameters(string? contentHash, string? contextId, string? component, string? fileArea, string? itemId,
        string? fileName, string? fileSize, string? mimeType, string filepath,
        string? timeCreated, string? timeModified, string? author, string? sortOrder, string? id)
    {
        Id = id;
        Contenthash = contentHash;
        Contextid = contextId;
        Component = component;
        Filearea = fileArea;
        Itemid = itemId;
        Filename = fileName;
        Filepath = filepath;
        Filesize = fileSize;
        Mimetype = mimeType;
        Timecreated = timeCreated;
        Timemodified = timeModified; 
        Source = fileName;
        Author = author;
        Sortorder = sortOrder;
    }
    
    [XmlElement(ElementName="contenthash")]
    public string? Contenthash { get; set; }
    
    [XmlElement(ElementName="contextid")]
    public string? Contextid { get; set; }
    
    [XmlElement(ElementName="component")]
    public string? Component { get; set; }
    
    [XmlElement(ElementName="filearea")]
    public string? Filearea { get; set; }
    
    [XmlElement(ElementName="itemid")]
    public string? Itemid { get; set; }
    
    [XmlElement(ElementName="filepath")]
    public string Filepath = "/";
    
    [XmlElement(ElementName="filename")]
    public string? Filename { get; set; }
    
    [XmlElement(ElementName="userid")]
    public string Userid = "$@NULL@$";
    
    [XmlElement(ElementName="filesize")]
    public string? Filesize { get; set; }
    
    [XmlElement(ElementName="mimetype")]
    public string? Mimetype { get; set; }
    
    [XmlElement(ElementName="status")]
    public string Status = "0";
    
    [XmlElement(ElementName="timecreated")]
    public string? Timecreated { get; set; }
    
    [XmlElement(ElementName="timemodified")]
    public string? Timemodified { get; set; }
    
    [XmlElement(ElementName="source")]
    public string? Source { get; set; }
    
    [XmlElement(ElementName="author")]
    public string? Author { get; set; }
    
    [XmlElement(ElementName="license")]
    public string License = "unknown";
    
    [XmlElement(ElementName="sortorder")]
    public string? Sortorder { get; set; }

    [XmlElement(ElementName = "repositorytype")]
    public string Repositorytype = "$@NULL@$";

    [XmlElement(ElementName = "repositoryid")]
    public string Repositoryid = "$@NULL@$";

    [XmlElement(ElementName = "reference")]
    public string Reference = "$@NULL@$";
    
    [XmlAttribute(AttributeName="id")]
    public string? Id { get; set; }
    
}