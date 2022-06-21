using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

[XmlRoot(ElementName="category")]
public partial class CourseCourseXmlCategory : ICourseCourseXmlCategory{

	public void SetParameters(string name, string description, string id)
	{
		Name = name;
		Description = description;
		Id = id;
	}
		
    [XmlElement(ElementName="name")]
    public string Name = "";

    [XmlElement(ElementName = "description")]
    public string Description = "$@NULL@$"; 
		
    [XmlAttribute(AttributeName="id")]
    public string Id = "";
}