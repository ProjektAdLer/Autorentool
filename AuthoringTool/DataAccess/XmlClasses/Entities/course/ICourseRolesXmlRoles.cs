namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

public interface ICourseRolesXmlRoles : IXmlSerializable
{
    void SetParameters(string roleOverrides, string roleAssignments);
}