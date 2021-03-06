using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="course")]
public partial class MoodleBackupXmlCourse : IMoodleBackupXmlCourse{

	
	public void SetParameters(string courseid, string? title, string directory)
	{
        Courseid = courseid;
		Title = title;
		Directory = directory;
	}
		
    [XmlElement(ElementName="courseid")]
    public string Courseid = "";
    [XmlElement(ElementName="title")]
    public string? Title = "";
    [XmlElement(ElementName = "directory")]
    public string Directory = "course";
}