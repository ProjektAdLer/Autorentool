using System.IO.Abstractions;
using System.Security.Cryptography;
using AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlFileManager : IXmlFileManager
{
    public string? fileSize { get; set; }
    public string? fileCheckSum { get; set; }
    
    public List<FilesXmlFile>? filesXmlFilesList ;
    
    private IFileSystem _fileSystem;

    public XmlFileManager(IFileSystem? fileSystem = null)
    {
        _fileSystem = fileSystem?? new FileSystem();
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
        byte[] byteFile = _fileSystem.File.ReadAllBytes(filepath);
        
        SHA1 sha1hash =  SHA1.Create();
        var comp = sha1hash.ComputeHash(byteFile);
        string hashCheckSum = string.Concat(comp.Select(b => b.ToString("x2")));

        fileCheckSum = hashCheckSum;
        fileSize = byteFile.Length.ToString();
    }

    public string GetHashCheckSum()
    {
        return fileCheckSum;
    }
    
    public string GetFileSize()
    {
        return fileSize;
    }
    
    /// <summary>
    /// Create needed folder for any file (only the first 2 letters of the hash value as foldername)
    /// and copy file to the created folder
    /// </summary>
    /// <param name="hashCheckSum"></param>
    public void CreateFolderAndFiles(string filepath, string? hashCheckSum)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        string? hashFolderName = hashCheckSum?.Substring(0, 2);
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "files", hashFolderName));

        if (hashFolderName != null)
        {
            string fileDestination = Path.Join("XMLFilesForExport", "files", hashFolderName.Substring(0,2), 
                hashCheckSum);
            if (!_fileSystem.File.Exists(fileDestination))
            {
                _fileSystem.File.Copy(filepath, fileDestination);
            }
        }
    }
}