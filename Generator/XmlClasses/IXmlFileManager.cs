using Generator.XmlClasses.Entities.Files.xml;

namespace Generator.XmlClasses;

public interface IXmlFileManager
{
    List<FilesXmlFile> GetXmlFilesList();
    
    void SetXmlFilesList(List<FilesXmlFile> list);
    
    void CalculateHashCheckSumAndFileSize(string filepath);
    
    void CreateFolderAndFiles(string filepath, string hashCheckSum);

    string GetHashCheckSum();


    string GetFileSize();

}