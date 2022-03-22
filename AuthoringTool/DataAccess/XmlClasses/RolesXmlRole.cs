using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="role")]
public partial class RolesXmlRole
{
    [XmlElement(ElementName = "name")] 
    public string Name=string.Empty;
        
    [XmlElement(ElementName = "shortname")]
    public string Shortname=string.Empty;
        
    [XmlElement(ElementName="nameincourse")]
    public string Nameincourse=string.Empty;

    [XmlElement(ElementName = "description")]
    public string Description=string.Empty;

    [XmlElement(ElementName = "sortorder")]
    public string Sortorder=string.Empty;

    [XmlElement(ElementName = "archetype")]
    public string Archetype=string.Empty;

    [XmlAttribute(AttributeName = "id")] 
    public string Id=string.Empty;
}