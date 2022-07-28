namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesRolesXmlRoles : IXmlSerializablePath
{
    string RoleOverrides { get; set; }
        
    string RoleAssignments { get; set; }
}