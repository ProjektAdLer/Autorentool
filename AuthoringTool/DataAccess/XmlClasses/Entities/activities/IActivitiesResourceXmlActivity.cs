namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesResourceXmlActivity : IXmlSerializablePath
{
    void SetParameters(ActivitiesResourceXmlResource? resource, string? id, string? moduleid, string? modulename,
        string? contextid);
}