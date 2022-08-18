using System.IO.Abstractions;
using System.Security.Cryptography;
using Generator.XmlClasses.Entities.Files.xml;

namespace Generator.XmlClasses;

public class XmlFileManager : IXmlFileManager
{
    private string _fileSize;
    private string _fileCheckSum;
    public List<FilesXmlFile> FilesXmlFilesList ;
    private readonly IFileSystem _fileSystem;

    public XmlFileManager(IFileSystem? fileSystem = null)
    {
        _fileSize = "";
        _fileCheckSum = "";
        _fileSystem = fileSystem?? new FileSystem();
        FilesXmlFilesList = new List<FilesXmlFile>();
    }
    
    public List<FilesXmlFile> GetXmlFilesList()
    {
        return FilesXmlFilesList;
    }

    public void SetXmlFilesList(List<FilesXmlFile> list)
    {
        FilesXmlFilesList = list;
    }
    
    /// <summary>
    /// Calculates the SHA1 Hash value for the file
    /// </summary>
    public void CalculateHashCheckSumAndFileSize(string filepath)
    {
        byte[] byteFile = _fileSystem.File.ReadAllBytes(filepath);
        
        SHA1 sha1Hash =  SHA1.Create();
        var comp = sha1Hash.ComputeHash(byteFile);
        string hashCheckSum = string.Concat(comp.Select(b => b.ToString("x2")));

        _fileCheckSum = hashCheckSum;
        _fileSize = byteFile.Length.ToString();
    }

    public string GetHashCheckSum()
    {
        return _fileCheckSum;
    }
    
    public string GetFileSize()
    {
        return _fileSize;
    }

    /// <summary>
    /// Create needed folder for any file (only the first 2 letters of the hash value as foldername)
    /// and copy file to the created folder
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="hashCheckSum"></param>
    public void CreateFolderAndFiles(string filepath, string hashCheckSum)
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        string hashFolderName = hashCheckSum.Substring(0, 2);
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "files", hashFolderName));
        
        string fileDestination = Path.Join("XMLFilesForExport", "files", hashFolderName.Substring(0, 2),
            hashCheckSum);
        if (!_fileSystem.File.Exists(fileDestination))
        {
            _fileSystem.File.Copy(filepath, fileDestination);
        }
    }
}