using System.Xml;
using System.Xml.Linq;
using AuthoringTool.DataAccess.WorldExport;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.course;
using AuthoringTool.DataAccess.XmlClasses.sections;



namespace AuthoringTool.DataAccess.WorldExport;

public class BackupFileGenerator : IBackupFileGenerator
{
    
    /// <summary>
    /// Creates all directories and XMl-Files needed for the Moodle backup
    /// </summary>
    public void WriteXMLFiles()
    {
        var currWorkDir = Directory.GetCurrentDirectory();
        Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport");
        Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport/course");
        Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport/sections");
        Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport/sections/section_160");
        
        var xmlEntityManager = new XmlEntityManager();
        xmlEntityManager.GetFactories();

    }
    
    //Get all files from source Folder "XMLFilesForExport" and pack all files and folders into a tar-file 
    //Afterwards pack the tar-file into a gzip file and rename the file to match the Moodle Backup format .mbz
    public void WriteBackupFile()
    {
        //copy template from current workdir
        var tempDir = GetTempDir();
        DirectoryCopy("XMLFilesForExport", tempDir);

        //construct tarball 
        const string tarName = "EmptyWorld.mbz";
        var tarPath = Path.Combine(tempDir, tarName);

        using (var outStream = File.Create(tarPath))
        using (var gzoStream = new GZipOutputStream(outStream))
        using (var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream))
        {
            tarArchive.RootPath = tempDir;
            SaveDirectoryToTar(tarArchive, tempDir, true);
        }

        //move file and delete dir
        if (File.Exists(tarName))
        {
            File.Delete(tarName);
        }

        File.Move(tarPath, tarName);
        Directory.Delete(tempDir, true);
    }

    /// <summary>
    /// Creates a temporary directory in the users temporary user folder and returns the path to it.
    /// </summary>
    /// <returns>Path to the temporary directory.</returns>
    public string GetTempDir()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        return tempDir;
    }

    /// <summary>
    /// Copies an entire directories contents into a target.
    /// </summary>
    /// <param name="source">The path of the folder which should be copied.</param>
    /// <param name="targetPrefix">The path of the target folder to which the contents should be copied.</param>
    public void DirectoryCopy(string source, string targetPrefix)
    {
        var directories = Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories);
        foreach (var directory in directories)
        {
            var directoryName = directory.Remove(0, (source + "\\").Length);
            Directory.CreateDirectory(Path.Combine(targetPrefix, directoryName));
        }
        var files = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories);
        foreach (var file in files) 
        {
            var filename = file.Remove(0, (source + "\\").Length);
            File.Copy(file, Path.Combine(targetPrefix, filename));
        }
    }
    
    /// <summary>
    /// Saves a directory and its contents (recursively) into a given tar archive.
    /// </summary>
    /// <param name="tar">The tar archive to which the directory and its contents should be saved.</param>
    /// <param name="source">The path of the source folder which should be saved to the archive.</param>
    /// <param name="recursive">Whether or not directories in the source should be saved recursively too.</param>
    private void SaveDirectoryToTar(TarArchive tar, string source, bool recursive)
    {
        TarEntry tarEntry = TarEntry.CreateEntryFromFile(source);
        tar.WriteEntry(tarEntry, false);
        var filenames = Directory.GetFiles(source).Where(s => !s.EndsWith(".mbz"));
        foreach (string filename in filenames)
        {
            tarEntry = TarEntry.CreateEntryFromFile(filename);
            tar.WriteEntry(tarEntry, false);
        }
        if (recursive)
        {
            string[] directories = Directory.GetDirectories(source);
            foreach (string directory in directories)
                SaveDirectoryToTar(tar, directory, recursive);
        }
    }
}

