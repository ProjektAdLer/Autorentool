using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="roles_definition")]
public partial class RolesXmlRolesDefinition : IRolesXmlRolesDefinition {

    public RolesXmlRolesDefinition()
    {
        Role = new RolesXmlRole();
    }
    
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "roles.xml");
    }
    
    [XmlElement(ElementName="role")]
    public RolesXmlRole Role { get; set; }
}