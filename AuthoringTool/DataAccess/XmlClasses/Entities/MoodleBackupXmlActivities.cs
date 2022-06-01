using System.Diagnostics;
using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;


[XmlRoot(ElementName="activities")]
public class MoodleBackupXmlActivities : IMoodleBackupXmlActivities {

    public void SetParameters(List<MoodleBackupXmlActivity>? moodleBackupXmlActivity)
    {
        Activity = moodleBackupXmlActivity;
    }
    
    
    [XmlElement(ElementName="activity")]
    public List<MoodleBackupXmlActivity>? Activity { get; set; }
}