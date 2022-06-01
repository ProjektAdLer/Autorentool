namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesH5PActivityXmlActivity : IXmlSerializablePath
{
    void SetParameterts(ActivitiesH5PActivityXmlH5PActivity? h5Pactivity, string? id, string? moduleid, string? modulename,
        string? contextid);
}