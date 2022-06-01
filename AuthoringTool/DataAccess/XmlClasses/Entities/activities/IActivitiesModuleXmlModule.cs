namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesModuleXmlModule : IXmlSerializablePath
{
    void SetParameterts(string? modulename, string? sectionid, string? sectionnumber, string? idnumber,
        string? added, string? score, string? indent, string? visible, string? visibleoncoursepage, string? visibleold,
        string? groupmode, string? groupingid, string? completion, string? completiongradeitemnumber, string? completionview,
        string? completionexpected, string? availability, string? showdescription, string? tags, string? id, string? version);
}