using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="section")]
public partial class MoodleBackupXmlSection
{
    [XmlElement(ElementName = "sectionid")]
    public string Sectionid = "";
		
    [XmlElement(ElementName="title")]
    public string Title = "";

    [XmlElement(ElementName = "directory")]
    public string Directory = "";
}