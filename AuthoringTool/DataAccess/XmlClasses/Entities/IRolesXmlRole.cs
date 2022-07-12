namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IRolesXmlRole
{
    void SetParameters(string name, string description, string id,
        string shortname, string nameincourse, string sortorder, string archetype);
}