using System.Security.Cryptography;
using AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlFileManager
{
    public string? fileSize { get; set; }
    public string? fileCheckSum { get; set; }
    
    public List<FilesXmlFile>? filesXmlFilesList ;

    public XmlFileManager()
    {
        List<FilesXmlFile> filesXmlFilesList = new List<FilesXmlFile>();
    }
    
    public List<FilesXmlFile>? GetXmlFilesList()
    {
        return filesXmlFilesList;
    }

    public void SetXmlFilesList(List<FilesXmlFile>? list)
    {
        filesXmlFilesList = list;
    }
    
    /// <summary>
    /// Calculates the SHA1 Hash value for the file
    /// </summary>
    public void CalculateHashCheckSumAndFileSize(string filepath)
    {
        byte[] byteFile = File.ReadAllBytes(filepath);
        
        SHA1 sha1hash =  SHA1.Create();
        var comp = sha1hash.ComputeHash(byteFile);
        string hashCheckSum = string.Concat(comp.Select(b => b.ToString("x2")));

        fileCheckSum = hashCheckSum;
        fileSize = byteFile.Length.ToString();
    }
    
    /// <summary>
    /// Create needed folder for any file (only the first 2 letters of the hash value as foldername)
    /// and copy file to the created folder
    /// </summary>
    /// <param name="hashCheckSum"></param>
    public void CreateFolderAndFiles(string filepath, string? hashCheckSum)
    {
        var currWorkDir = Directory.GetCurrentDirectory();
        string? hashFolderName = hashCheckSum?.Substring(0, 2);
        Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "files", hashFolderName));

        if (hashFolderName != null)
        {
            string fileDestination = Path.Join("XMLFilesForExport", "files", hashFolderName.Substring(0,2), 
                hashCheckSum);
            if (!File.Exists(fileDestination))
            {
                File.Copy(filepath, fileDestination);
            }
        }
    }
}