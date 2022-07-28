
namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IFilesXmlFiles : IXmlSerializable
{

    List<FilesXmlFile> File { get; set; }
}