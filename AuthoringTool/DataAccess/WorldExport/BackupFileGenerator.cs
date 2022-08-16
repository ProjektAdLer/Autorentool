using System.IO.Abstractions;
using AuthoringTool.DataAccess.DSL;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using AuthoringTool.DataAccess.XmlClasses;
using FileSystem = System.IO.Abstractions.FileSystem;


namespace AuthoringTool.DataAccess.WorldExport;

/// <summary>
/// The BackupFileGenerator creates all the folders, xml files and packs it to a .mbz file.
/// The file will be imported to moodle.
/// </summary>
public class BackupFileGenerator : IBackupFileGenerator
{
    
    private IFileSystem _fileSystem;
    public IXmlEntityManager xmlEntityManager;

    
    // Constructor for tests. fileSystem makes it possible to test various System methods that write files on disk.
    public BackupFileGenerator(IFileSystem? fileSystem = null, IXmlEntityManager entityManager = null)
    {
        _fileSystem = fileSystem ?? new FileSystem();
        xmlEntityManager = entityManager ?? new XmlEntityManager();
    }

    ///<inheritdoc cref="IBackupFileGenerator.CreateBackupFolders"/>
    public void CreateBackupFolders()
    {
        var currWorkDir = _fileSystem.Directory.GetCurrentDirectory();
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport"));
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities"));
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "files"));
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "course"));
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections"));
    }

    /// <inheritdoc cref="IBackupFileGenerator.WriteXmlFiles"/>
    public void WriteXmlFiles(IReadDsl? readDsl, string dslpath)
    {
        if (readDsl != null) xmlEntityManager.GetFactories(readDsl, dslpath);
    }
    
    // Get all files from source Folder "XMLFilesForExport" and pack all files and folders into a tar-file 
    // Afterwards pack the tar-file into a gzip file and rename the file to match the Moodle Backup format .mbz
    /// <inheritdoc cref="IBackupFileGenerator.WriteBackupFile"/>
    public void WriteBackupFile(string filepath)
    {
        //copy template from current workdir
        var tempDir = GetTempDir();
        DirectoryCopy("XMLFilesForExport", tempDir);

        //construct tarball 
        const string tarName = "EmptyWorld.mbz";
        var tarPath = _fileSystem.Path.Combine(tempDir, tarName);

        using (var outStream = _fileSystem.File.Create(tarPath))
        using (var gzoStream = new GZipOutputStream(outStream))
        using (var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream))
        {
            tarArchive.RootPath = tempDir;
            SaveDirectoryToTar(tarArchive, tempDir, true);
        }

        //move file and delete dir
        if (_fileSystem.File.Exists(tarName))
        {
            _fileSystem.File.Delete(tarName);
        }

        _fileSystem.File.Move(tarPath, filepath);
        _fileSystem.Directory.Delete(tempDir, true);
        _fileSystem.Directory.Delete("XMLFilesForExport", true);
    }

    /// <summary>
    /// Creates a temporary directory in the users temporary user folder and returns the path to it.
    /// </summary>
    /// <returns>Path to the temporary directory.</returns>
    public string GetTempDir()
    {
        var tempDir = _fileSystem.Path.Combine(_fileSystem.Path.GetTempPath(), _fileSystem.Path.GetRandomFileName());
        _fileSystem.Directory.CreateDirectory(tempDir);
        return tempDir;
    }

    /// <summary>
    /// Copies an entire directories contents into a target.
    /// </summary>
    /// <param name="source">The path of the folder which should be copied.</param>
    /// <param name="targetPrefix">The path of the target folder to which the contents should be copied.</param>
    public void DirectoryCopy(string source, string targetPrefix)
    {
        var directories = _fileSystem.Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories);
        foreach (var directory in directories)
        {
            var directoryName = directory.Remove(0, _fileSystem.Path.Join(source, "\\").Length);
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.Combine(targetPrefix, directoryName));
        }
        var files = _fileSystem.Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories);
        foreach (var file in files) 
        {
            var filename = file.Remove(0, _fileSystem.Path.Join(source, "\\").Length);
            _fileSystem.File.Copy(file, _fileSystem.Path.Combine(targetPrefix, filename));
        }
    }
    
    /// <summary>
    /// Saves a directory and its contents (recursively) into a given tar archive.
    /// </summary>
    /// <param name="tar">The tar archive to which the directory and its contents should be saved.</param>
    /// <param name="source">The path of the source folder which should be saved to the archive.</param>
    /// <param name="recursive">Whether or not directories in the source should be saved recursively too.</param>
    public void SaveDirectoryToTar(TarArchive tar, string source, bool recursive)
    {
        TarEntry tarEntry = TarEntry.CreateEntryFromFile(source);
        tar.WriteEntry(tarEntry, false);
        var filenames = _fileSystem.Directory.GetFiles(source).Where(s => !s.EndsWith(".mbz"));
        foreach (string filename in filenames)
        {
            tarEntry = TarEntry.CreateEntryFromFile(filename);
            tar.WriteEntry(tarEntry, false);
        }
        if (recursive)
        {
            string[] directories = _fileSystem.Directory.GetDirectories(source);
            foreach (string directory in directories)
                SaveDirectoryToTar(tar, directory, recursive);
        }
    }
}

