using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="details")]
public partial class MoodleBackupXmlDetails {
    [XmlElement(ElementName="detail")]
    public MoodleBackupXmlDetail Detail;
}