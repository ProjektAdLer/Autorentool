namespace AuthoringTool.DataAccess.XmlClasses.Entities.Course.Roles.xml;

public interface ICourseRolesXmlRoles : IXmlSerializable
{
    string RoleOverrides { get; set; }

    string RoleAssignments { get; set; }
}