namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IRolesXmlRolesDefinition : IXmlSerializable
{
    RolesXmlRole Role { get; set; }
}