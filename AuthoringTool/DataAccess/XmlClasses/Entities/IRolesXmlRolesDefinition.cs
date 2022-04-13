namespace AuthoringTool.DataAccess.XmlClasses;

public interface IRolesXmlRolesDefinition : IXmlSerializable
{
    void SetParameters(RolesXmlRole? rolesRole);
}