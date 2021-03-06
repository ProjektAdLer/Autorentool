using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="grade_setting")]
public partial class GradebookXmlGradeSetting : IGradebookXmlGradeSetting {
    
    public void SetParameters(string name, string value, string id)
    {
        Name = name;
        Value = value;
        Id = id;
    }
    
    [XmlElement(ElementName="name")]
    public string Name = "";
        
    [XmlElement(ElementName="value")]
    public string Value = "";
        
    [XmlAttribute(AttributeName="id")]
    public string Id = "";
}