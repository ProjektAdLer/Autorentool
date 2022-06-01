namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesRolesXmlRoles : IXmlSerializablePath
{
    void SetParameterts(string? roleOverrides, string? roleAssignments);
}