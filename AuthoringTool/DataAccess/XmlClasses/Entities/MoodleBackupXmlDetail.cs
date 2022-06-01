using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="detail")]
public partial class MoodleBackupXmlDetail : IMoodleBackupXmlDetail{
	public void SetParameters(string? type, string? format, string? interactive, string? mode, 
		string? execution, string? executiontime, string? backupId)
	{
		Type = type;
		Format = format;
		Interactive = interactive;
		Mode = mode;
		Execution = execution;
		Executiontime = executiontime;
		Backup_id = backupId;
	}

	[XmlElement(ElementName="type")]
	public string? Type { get; set; }
	[XmlElement(ElementName="format")]
	public string? Format { get; set; }
	[XmlElement(ElementName="interactive")]
	public string? Interactive { get; set; }
	[XmlElement(ElementName="mode")]
	public string? Mode { get; set; }
	[XmlElement(ElementName="execution")]
	public string? Execution { get; set; }
	[XmlElement(ElementName="executiontime")]
	public string? Executiontime { get; set; }
	[XmlAttribute(AttributeName="backup_id")]
	public string? Backup_id { get; set; }
}