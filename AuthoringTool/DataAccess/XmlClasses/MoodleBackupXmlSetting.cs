using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="setting")]
public partial class MoodleBackupXmlSetting {
    [XmlElement(ElementName="level")]
    public string Level = "root";
		
    [XmlElement(ElementName="name")]
    public string Name = "";
		
    [XmlElement(ElementName="value")]
    public string Value = "";
		
    [XmlElement(ElementName="section")]
    public string Section = "";
}