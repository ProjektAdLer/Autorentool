using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="category")]
public partial class CourseCourseXmlCategory {

	public CourseCourseXmlCategory()
	{
		Name = "Miscellaneous";
		Id = "1";
	}
		
    [XmlElement(ElementName="name")]
    public string Name = "";

    [XmlElement(ElementName = "description")]
    public string Description = "$@NULL@$"; 
		
    [XmlAttribute(AttributeName="id")]
    public string Id = "";
}