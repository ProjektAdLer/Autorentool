using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="detail")]
public partial class MoodleBackupXmlDetail : IMoodleBackupXmlDetail{


	public void SetParameters(string backupId)
	{
        Backup_id = backupId;
	}
		
    [XmlElement(ElementName="type")]
    public string Type = "course";
		
    [XmlElement(ElementName="format")]
    public string Format = "moodle2";

    [XmlElement(ElementName = "interactive")]
    public string Interactive = "1";
		
    [XmlElement(ElementName="mode")]
    public string Mode = "10";

    [XmlElement(ElementName = "execution")]
    public string Execution = "1";

    [XmlElement(ElementName = "executiontime")]
    public string Executiontime = "0";

    [XmlAttribute(AttributeName = "backup_id")]
    public string Backup_id = "";
}