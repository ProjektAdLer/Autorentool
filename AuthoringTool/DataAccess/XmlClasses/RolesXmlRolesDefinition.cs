using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="roles_definition")]
public partial class RolesXmlRolesDefinition {
    [XmlElement(ElementName="role")]
    public RolesXmlRole Role { get; set; }
}