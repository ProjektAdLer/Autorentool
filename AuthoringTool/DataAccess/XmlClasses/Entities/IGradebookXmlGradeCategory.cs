namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IGradebookXmlGradeCategory
{
    void SetParameters(string? parent, string? depth, string? path, string? fullname, string? aggregation,
        string? keephigh, string? droplow, string? aggregateonlygraded, string? aggregateoutcomes, string? timecreated,
        string? timemodified, string? hidden, string? id);
}