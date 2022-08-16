using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[XmlRoot(ElementName="setting")]
public class MoodleBackupXmlSetting : IMoodleBackupXmlSetting{

	public MoodleBackupXmlSetting()
	{
		Name = "";
		Value = "0";
		Level = "root";
	}
	
	public MoodleBackupXmlSetting(string level, string name, string value, string settingType, bool isSection)
	{
      Name = name;
      Value = value; 
      Level = level;
      if (isSection)
      {
	      Section = settingType;
      }
      else 
      {
	      Activity = settingType;
      }
	      
	}

	[XmlElement(ElementName="level")]
	public string Level { get; set; }
	
	[XmlElement(ElementName="name")]
	public string Name { get; set; }
	
	[XmlElement(ElementName="value")]
	public string Value { get; set; }
	
	[XmlElement(ElementName="section")]
	public string Section { get; set; }
	
	[XmlElement(ElementName="activity")]
	public string Activity { get; set; }
}