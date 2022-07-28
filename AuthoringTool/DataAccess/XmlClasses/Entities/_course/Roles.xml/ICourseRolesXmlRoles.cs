namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

public interface ICourseRolesXmlRoles : IXmlSerializable
{
    string RoleOverrides { get; set; }

    string RoleAssignments { get; set; }
}