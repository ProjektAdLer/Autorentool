using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="grade_setting")]
public partial class GradebookXmlGradeSetting : IGradebookXmlGradeSetting {

    public GradebookXmlGradeSetting()
    {
        Name = "minmaxtouse";
        Value = "1";
        Id = "";
    }

    [XmlElement(ElementName="name")]
    public string Name { get; set; }
        
    [XmlElement(ElementName="value")]
    public string Value { get; set; }
        
    [XmlAttribute(AttributeName="id")]
    public string Id { get; set; }
}