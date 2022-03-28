using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="details")]
public partial class MoodleBackupXmlDetails {

   
    public void SetParameters(MoodleBackupXmlDetail moodlebackupDetail)
    {
        Detail = moodlebackupDetail;
    }
    
    [XmlElement(ElementName="detail")]
    public MoodleBackupXmlDetail Detail;
}