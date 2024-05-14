using System.IO.Abstractions;
using System.Security.Cryptography;
using Generator.XmlClasses.Entities.Files.xml;
using Shared.Configuration;

namespace Generator.XmlClasses;

public class XmlFileManager : IXmlFileManager
{
    private readonly IFileSystem _fileSystem;
    private string _fileCheckSum;
    private string _fileSize;
    public List<FilesXmlFile> FilesXmlFilesList;

    public XmlFileManager(IFileSystem? fileSystem = null)
    {
        _fileSize = "";
        _fileCheckSum = "";
        _fileSystem = fileSystem ?? new FileSystem();
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
        var byteFile = _fileSystem.File.ReadAllBytes(filepath);

        var sha1Hash = SHA1.Create();
        var comp = sha1Hash.ComputeHash(byteFile);
        var hashCheckSum = string.Concat(comp.Select(b => b.ToString("x2")));

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
    /// Create needed folder for any file (only the first 2 letters of the hash value as folder name)
    /// and copy file to the created folder
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="hashCheckSum"></param>
    public void CreateFolderAndFiles(string filepath, string hashCheckSum)
    {
        var currWorkDir =  ApplicationPaths.BackupFolder;
        var hashFolderName = hashCheckSum.Substring(0, 2);
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "files", hashFolderName));

        var fileDestination = Path.Join(currWorkDir, "XMLFilesForExport", "files", hashFolderName.Substring(0, 2),
            hashCheckSum);
        if (!_fileSystem.File.Exists(fileDestination))
        {
            _fileSystem.File.Copy(filepath, fileDestination);
        }

        _fileSystem.File.Delete(filepath);
    }
}