using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="sections")]
public partial class MoodleBackupXmlSections {
    [XmlElement(ElementName="section")]
    public MoodleBackupXmlSection Section;
}