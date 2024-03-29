﻿using Generator.ATF;
using ICSharpCode.SharpZipLib.Tar;

namespace Generator.WorldExport;

public interface IBackupFileGenerator
{
    /// <summary>
    /// Creates all Folders needed for the Backup File
    /// </summary>
    void CreateBackupFolders();

    /// <summary>
    /// Creates all Xml Files at the right location.
    /// </summary>
    void WriteXmlFiles(IReadAtf readAtf);

    /// <summary>
    /// Locates all Folders and Xml Files, packs it into a tar.gzip file and renames it to .mbz (moodle backup zip) 
    /// </summary>
    void WriteBackupFile(string filepath);

    /// <summary>
    /// create a temporary directory for the folders and xml files
    /// </summary>
    /// <returns></returns>
    string GetTempDir();

    /// <summary>
    /// Copy a Directory to the target location
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetPrefix"></param>
    void DirectoryCopy(string source, string targetPrefix);

    /// <summary>
    /// Zip a directory to a .mbz file (tar + gzip)
    /// </summary>
    /// <param name="tar"></param>
    /// <param name="source"></param>
    /// <param name="recursive"></param>
    void SaveDirectoryToTar(TarArchive tar, string source, bool recursive);

    /// <summary>
    /// Extracts the .atf file from the .mbz file and returns the path to the .atf file
    /// </summary>
    /// <param name="filepath">Path of the .mbz file.</param>
    /// <returns>Path to the extracted .atf file</returns>
    string ExtractAtfFromBackup(string filepath);
}