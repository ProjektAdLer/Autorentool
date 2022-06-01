namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesGradesXmlGradeItem
{
    void SetParameters(string? categoryid, string? itemname, string? itemtype, string? itemmodule, string? iteminstance,
        string? itemnumber, string? iteminfo, string? idnumber, string? calculation, string? gradetype, string? grademax,
        string? grademin, string? scaleid, string? outcomeid, string? gradepass, string? multfactor, string? plusfactor,
        string? aggregationcoef, string? aggregationcoef2, string? weightoverride, string? sortorder, string? display,
        string? decimals, string? hidden, string? locked, string? locktime, string? needsupdate, string? timecreated,
        string? timemodified, string? gradeGrades, string? id);
}