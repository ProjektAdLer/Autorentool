namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IFilesXmlFile
{
    void SetParameters(string? contentHash, string? contextId, string? component, string? fileArea, string? itemId,
        string? fileName, string? fileSize, string? mimeType, string filepath,
        string? timeCreated, string? timeModified, string? author, string? sortOrder, string? id);
}