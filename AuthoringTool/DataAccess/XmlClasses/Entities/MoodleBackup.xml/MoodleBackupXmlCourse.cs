using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="course")]
public partial class MoodleBackupXmlCourse : IMoodleBackupXmlCourse{

	public MoodleBackupXmlCourse()
	{
		CourseId = "1";
		Title = "";
		Directory = "course";
	}
	
    [XmlElement(ElementName="courseid")]
    public string CourseId { get; set; }
    
    [XmlElement(ElementName="title")]
    public string Title { get; set; }
    
    [XmlElement(ElementName = "directory")]
    public string Directory { get; set; }
}