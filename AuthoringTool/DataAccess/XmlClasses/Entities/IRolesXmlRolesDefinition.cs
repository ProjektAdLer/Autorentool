namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IRolesXmlRolesDefinition : IXmlSerializable
{
    void SetParameters(RolesXmlRole? rolesRole);
}