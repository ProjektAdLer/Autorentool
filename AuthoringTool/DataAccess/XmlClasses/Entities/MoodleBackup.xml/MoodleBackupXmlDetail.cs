using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="detail")]
public partial class MoodleBackupXmlDetail : IMoodleBackupXmlDetail{

	public MoodleBackupXmlDetail()
	{
		Type = "course";
		Format = "moodle2";
		Interactive = "1";
		Mode = "10";
		Execution = "1";
		ExecutionTime = "0";
		BackupId = "36d63c7b4624cf6a79e0405be770974d";
	}
	

	[XmlElement(ElementName="type")]
	public string Type { get; set; }
	
	[XmlElement(ElementName="format")]
	public string Format { get; set; }
	
	[XmlElement(ElementName="interactive")]
	public string Interactive { get; set; }
	
	[XmlElement(ElementName="mode")]
	public string Mode { get; set; }
	
	[XmlElement(ElementName="execution")]
	public string Execution { get; set; }
	
	[XmlElement(ElementName="executiontime")]
	public string ExecutionTime { get; set; }
	
	[XmlAttribute(AttributeName="backup_id")]
	public string BackupId { get; set; }
}