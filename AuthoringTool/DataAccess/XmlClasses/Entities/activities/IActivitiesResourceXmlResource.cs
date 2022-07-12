namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesResourceXmlResource
{
    void SetParameters(string? name, string? intro, string? introformat, string? tobemigrated, string? legacyfiles,
        string? legacyfileslast, string? display, string? displayoptions, string? filterfiles, string? revision,
        string? timemodified, string? id);
}