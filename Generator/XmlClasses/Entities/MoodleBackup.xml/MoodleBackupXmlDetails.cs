using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.MoodleBackup.xml;

[XmlRoot(ElementName="details")]
public class MoodleBackupXmlDetails : IMoodleBackupXmlDetails {

    public MoodleBackupXmlDetails()
    {
        Detail = new MoodleBackupXmlDetail();
    }
    
    [XmlElement(ElementName="detail")]
    public MoodleBackupXmlDetail Detail { get; set; }
}