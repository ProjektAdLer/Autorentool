using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="details")]
public partial class MoodleBackupXmlDetails : IMoodleBackupXmlDetails {

    public MoodleBackupXmlDetails()
    {
        Detail = new MoodleBackupXmlDetail();
    }
    
    [XmlElement(ElementName="detail")]
    public MoodleBackupXmlDetail Detail { get; set; }
}