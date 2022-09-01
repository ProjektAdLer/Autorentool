namespace Generator.XmlClasses.Entities._course.Roles.xml;

public interface ICourseRolesXmlRoles : IXmlSerializable
{
    string RoleOverrides { get; set; }

    string RoleAssignments { get; set; }
}