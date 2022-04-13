namespace AuthoringTool.DataAccess.XmlClasses.course;

public interface ICourseRolesXmlRoles : IXmlSerializable
{
    void SetParameters(string roleOverrides, string roleAssignments);
}