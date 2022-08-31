using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._course.Course.xml;

[XmlRoot(ElementName="category")]
public class CourseCourseXmlCategory : ICourseCourseXmlCategory{


	public CourseCourseXmlCategory()
	{
		Name = "Miscellaneous";
		Description = "$@NULL@$";
		Id = "1";
	}
	
		
    [XmlElement(ElementName="name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "description")]
    public string Description { get; set; }
		
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
}