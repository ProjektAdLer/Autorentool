namespace Generator.XmlClasses.Entities._activities.Roles.xml;

public interface IActivitiesRolesXmlRoles : IXmlSerializablePath
{
    string RoleOverrides { get; set; }
        
    string RoleAssignments { get; set; }
}