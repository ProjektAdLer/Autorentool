using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="role")]
public partial class RolesXmlRole : IRolesXmlRole
{

    public RolesXmlRole()
    {
        Name = "";
        Description = "";
        Id = "5";
        Shortname = "student";
        NameInCourse = "$@NULL@$";
        Sortorder = "5";
        Archetype = "student";
    }

    [XmlElement(ElementName = "name")] 
    public string Name { get; set; }
        
    [XmlElement(ElementName = "shortname")]
    public string Shortname { get; set; }
        
    [XmlElement(ElementName="nameincourse")]
    public string NameInCourse { get; set; }

    [XmlElement(ElementName = "description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "sortorder")]
    public string Sortorder { get; set; }

    [XmlElement(ElementName = "archetype")]
    public string Archetype { get; set; }

    [XmlAttribute(AttributeName = "id")] 
    public string Id { get; set; }
}