using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="setting")]
public partial class MoodleBackupXmlSetting : IMoodleBackupXmlSetting{


	public void SetParametersShort(string name, string value)
	{
        Name = name;
		Value = value;
	}
	
	public void SetParametersFull(string name, string value, string level, string section)
	{
      Name = name;
      Value = value; 
      Level = level;
      Section = section;  
	}
	
    [XmlElement(ElementName="level")]
    public string Level = "root";
		
    [XmlElement(ElementName="name")]
    public string Name = "";
		
    [XmlElement(ElementName="value")]
    public string Value = "";
		
    [XmlElement(ElementName="section")]
    public string Section = "";
}