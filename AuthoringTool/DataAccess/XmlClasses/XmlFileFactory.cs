using System.Security.Cryptography;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlFileFactory
{
    

    public void CreateXmlFileFactory()
    {
        string hashCheckSum = CreateFoldersForFiles();
        CreateFilesInFolder(hashCheckSum);
    }
    
    private string filepath = Path.Join("C:", "Users", "biglerdd", "Desktop", "HS", "Hagel-Lernelemente", "Wortsuche Metriken.h5p");

    /// <summary>
    /// Creates the needed Folder for the File in the BackupFolder Structure
    /// </summary>
    public string CreateFoldersForFiles()
    {
        byte[] byteFile = File.ReadAllBytes(filepath);

        var hash = new SHA1Managed().ComputeHash(byteFile);
        string hashCheckSum = string.Concat(hash.Select(b => b.ToString("x2")));
        string hashFolderName = hashCheckSum.Substring(0, 2);
        
        var currWorkDir = Directory.GetCurrentDirectory();
        Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "files", hashFolderName));
        return hashCheckSum;
    }

    public void CreateFilesInFolder(string hashFolderName)
    {
        string destination = Path.Join("XMLFilesForExport", "files", hashFolderName.Substring(0,2), hashFolderName);
        File.Copy(filepath, destination);
        //File.Move(destination, hashFolderName);
    }
}