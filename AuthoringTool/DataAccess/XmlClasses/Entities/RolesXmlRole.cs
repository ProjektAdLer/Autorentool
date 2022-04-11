using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="role")]
public partial class RolesXmlRole : IRolesXmlRole
{
    
    public void SetParameters(string name, string description, string id,
                                      string shortname, string nameincourse, string sortorder, string archetype)
    {
        Name = name;
        Description = description;
        Id = id;
        Shortname = shortname;
        Nameincourse = nameincourse;
        Sortorder = sortorder;
        Archetype = archetype;
    }
    
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