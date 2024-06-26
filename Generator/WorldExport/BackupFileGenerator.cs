﻿using System.IO.Abstractions;
using Generator.ATF;
using Generator.XmlClasses;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using Shared.Configuration;

namespace Generator.WorldExport;

/// <summary>
/// The BackupFileGenerator creates all the folders, xml files and packs it to a .mbz file.
/// The file will be imported to moodle.
/// </summary>
public class BackupFileGenerator : IBackupFileGenerator
{
    private readonly IFileSystem _fileSystem;
    public readonly IXmlEntityManager XmlEntityManager;


    // Constructor for tests. fileSystem makes it possible to test various System methods that write files on disk.
    public BackupFileGenerator(IFileSystem? fileSystem = null, IXmlEntityManager? entityManager = null)
    {
        _fileSystem = fileSystem ?? new FileSystem();
        XmlEntityManager = entityManager ?? new XmlEntityManager();
    }

    ///<inheritdoc cref="IBackupFileGenerator.CreateBackupFolders"/>
    public void CreateBackupFolders()
    {
        var currWorkDir = ApplicationPaths.BackupFolder;
        var xmlFilesBasePath = Path.Join(currWorkDir, "XMLFilesForExport");
        if (_fileSystem.Directory.Exists(xmlFilesBasePath)) return;
        _fileSystem.Directory.CreateDirectory(xmlFilesBasePath);
        _fileSystem.Directory.CreateDirectory(Path.Join(xmlFilesBasePath, "activities"));
        _fileSystem.Directory.CreateDirectory(Path.Join(xmlFilesBasePath, "files"));
        _fileSystem.Directory.CreateDirectory(Path.Join(xmlFilesBasePath, "course"));
        _fileSystem.Directory.CreateDirectory(Path.Join(xmlFilesBasePath, "sections"));
    }

    /// <inheritdoc cref="IBackupFileGenerator.WriteXmlFiles"/>
    public void WriteXmlFiles(IReadAtf readAtf)
    {
        XmlEntityManager.GetFactories(readAtf);
    }

    // Get all files from source Folder "XMLFilesForExport" and pack all files and folders into a tar-file 
    // Afterwards pack the tar-file into a gzip file and rename the file to match the Moodle Backup format .mbz
    /// <inheritdoc cref="IBackupFileGenerator.WriteBackupFile"/>
    public void WriteBackupFile(string filepath)
    {
        string? tempDir = null;
        try
        {
            //copy template from current workdir
            tempDir = GetTempDir();
            DirectoryCopy(Path.Combine(ApplicationPaths.BackupFolder,"XMLFilesForExport"), tempDir);

            //construct tarball 
            const string tarName = "EmptyWorld.mbz";
            var tarPath = _fileSystem.Path.Combine(tempDir, tarName);

            using (var outStream = _fileSystem.File.Create(tarPath))
            using (var gzoStream = new GZipOutputStream(outStream))
            using (var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream))
            {
                //we need to remove the first slash from our rootDir, because absolute paths are not allowed in the tar entries
                //and the sharpziplib removes them from the entry names. this is a bug in the library.
                var rootDir = tempDir;
                while (rootDir.StartsWith("/"))
                    rootDir = rootDir.Substring(1);
                tarArchive.RootPath = rootDir;
                SaveDirectoryToTar(tarArchive, tempDir, true);
            }

            //delete tar
            if (_fileSystem.File.Exists(tarName))
            {
                _fileSystem.File.Delete(tarName);
            }

            //move file
            _fileSystem.File.Move(tarPath, filepath, true);
        }
        finally
        {
            //clean up directory
            if (tempDir != null)
                _fileSystem.Directory.Delete(tempDir, true);
        }
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
            var relativeDirectoryPath = _fileSystem.Path.GetRelativePath(source, directory);
            _fileSystem.Directory.CreateDirectory(_fileSystem.Path.Combine(targetPrefix, relativeDirectoryPath));
        }

        var files = _fileSystem.Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var relativeFilepath = _fileSystem.Path.GetRelativePath(source, file);
            _fileSystem.File.Copy(file, _fileSystem.Path.Combine(targetPrefix, relativeFilepath));
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
        var tarEntry = TarEntry.CreateEntryFromFile(source);
        tar.WriteEntry(tarEntry, false);
        var filenames = _fileSystem.Directory.GetFiles(source).Where(s => !s.EndsWith(".mbz"));
        foreach (var filename in filenames)
        {
            tarEntry = TarEntry.CreateEntryFromFile(filename);
            tar.WriteEntry(tarEntry, false);
        }

        if (recursive)
        {
            string[] directories = _fileSystem.Directory.GetDirectories(source);
            foreach (var directory in directories)
                SaveDirectoryToTar(tar, directory, recursive);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public string ExtractAtfFromBackup(string filepath)
    {
        using var inStream = _fileSystem.File.OpenRead(filepath);
        using var gziStream = new GZipInputStream(inStream);
        using var tarStream = new TarInputStream(gziStream, null);
        while (tarStream.GetNextEntry() is { } entry)
        {
            if (entry.IsDirectory) continue;
            if (!entry.Name.EndsWith(".json")) continue;

            var tempDir = GetTempDir();
            var tempPath = _fileSystem.Path.Combine(tempDir, entry.Name);
            using var outStream = _fileSystem.File.Create(tempPath);
            tarStream.CopyEntryContents(outStream);

            return tempPath;
        }

        throw new ApplicationException("No .atf file found in backup.");
    }
}