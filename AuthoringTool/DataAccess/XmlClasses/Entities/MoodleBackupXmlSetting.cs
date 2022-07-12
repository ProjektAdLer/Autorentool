using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="setting")]
public partial class MoodleBackupXmlSetting : IMoodleBackupXmlSetting{


	public void SetParametersSetting(string? level, string? name, string? value)
	{
		Level = level;
        Name = name;
		Value = value;
	}
	
	public void SetParametersSection(string? level, string? section, string? name, string? value)
	{
      Name = name;
      Value = value; 
      Level = level;
      Section = section;  
	}
	
	public void SetParametersActivity(string? level, string? activity, string? name, string? value)
	{
		Name = name;
		Value = value; 
		Level = level;
		Activity = activity;  
	}
	
	[XmlElement(ElementName="level")]
	public string? Level { get; set; }
	
	[XmlElement(ElementName="name")]
	public string? Name { get; set; }
	
	[XmlElement(ElementName="value")]
	public string? Value { get; set; }
	
	[XmlElement(ElementName="section")]
	public string? Section { get; set; }
	
	[XmlElement(ElementName="activity")]
	public string? Activity { get; set; }
}