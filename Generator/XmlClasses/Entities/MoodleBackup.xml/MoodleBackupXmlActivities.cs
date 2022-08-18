using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities.MoodleBackup.xml;


[XmlRoot(ElementName="activities")]
public class MoodleBackupXmlActivities : IMoodleBackupXmlActivities {

    public MoodleBackupXmlActivities()
    {
        Activity = new List<MoodleBackupXmlActivity>();
    }
    
    
    [XmlElement(ElementName="activity")]
    public List<MoodleBackupXmlActivity> Activity { get; set; }
}