using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="grade_setting")]
public partial class GradebookXmlGradeSetting {
    [XmlElement(ElementName="name")]
    public string Name = "";
        
    [XmlElement(ElementName="value")]
    public string Value = "";
        
    [XmlAttribute(AttributeName="id")]
    public string Id = "";
}