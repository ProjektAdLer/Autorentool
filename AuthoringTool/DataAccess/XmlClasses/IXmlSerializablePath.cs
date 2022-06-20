namespace AuthoringTool.DataAccess.XmlClasses;

public interface IXmlSerializablePath
{
    void Serialize(string? activityName, string? module_sectionId);
}