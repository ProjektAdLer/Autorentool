using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="course")]
public partial class MoodleBackupXmlCourse {
		
    [XmlElement(ElementName="courseid")]
    public string Courseid = "";
    [XmlElement(ElementName="title")]
    public string Title = "";
    [XmlElement(ElementName = "directory")]
    public string Directory = "course";
}