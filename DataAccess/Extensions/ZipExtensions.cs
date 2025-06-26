using System.IO.Abstractions;
using System.IO.Compression;


namespace DataAccess.Extensions;

public static class ZipExtensions
{
    /// <summary>
    /// Opens a zip archive in read mode.
    /// </summary>
    /// <param name="fileSystemReference">The file system abstraction used to access the file.</param>
    /// <param name="archivePath">The path to the zip archive file.</param>
    /// <returns>A <see cref="ZipArchive"/> instance representing the archive.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fileSystemReference"/> or <paramref name="archivePath"/> is null.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permissions.</exception>
    /// <exception cref="InvalidDataException">Thrown if the file is not a valid zip archive.</exception>
    public static ZipArchive GetZipArchive(IFileSystem fileSystemReference, string archivePath)
    {
        ValidatePath(archivePath);
        var zipStream = fileSystemReference.File.OpenRead(archivePath);
        return new ZipArchive(zipStream, ZipArchiveMode.Read);
    }

    private static ZipArchive GetWritableZipArchive(IFileSystem fileSystemReference, string archivePath)
    {
        ValidatePath(archivePath);
        var zipStream = fileSystemReference.File.OpenWrite(archivePath);
        return new ZipArchive(zipStream, ZipArchiveMode.Create);
    }

    /// <summary>
    /// Extracts all entries from a zip archive to the specified directory.
    /// </summary>
    /// <param name="archive">The zip archive to extract.</param>
    /// <param name="fileSystemReference">The file system abstraction used to access files and directories.</param>
    /// <param name="destination">The directory to extract the archive's contents to.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="destination"/> is null.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have write access to the destination directory.</exception>
    /// <exception cref="IOException">Thrown if an I/O error occurs during extraction.</exception>
    public static void ExtractToDirectory(this ZipArchive archive, IFileSystem fileSystemReference, string destination)
    {
        EnsureDestinationDirectoryExists(fileSystemReference, destination);

        foreach (var entry in archive.Entries)
        {
            var extractedEntryPath = NormalizeExtractedEntryPathForWindowsAndUnix(entry);
            var fullPath = Path.Combine(destination, extractedEntryPath);
            CreateEntryDirectory(fileSystemReference, fullPath);

            if (fileSystemReference.File.Exists(fullPath))
                fileSystemReference.File.Delete(fullPath);

            using var destinationStream = fileSystemReference.File.Create(fullPath);
            using var sourceStream = entry.Open();
            sourceStream.CopyTo(destinationStream);
        }
    }

    /// <summary>
    /// Creates a zip archive from the contents of a directory asynchronously.
    /// </summary>
    /// <param name="fileSystemReference">The file system abstraction used to access files and directories.</param>
    /// <param name="source">The source directory to compress.</param>
    /// <param name="destination">The path of the resulting zip archive.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="destination"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> or <paramref name="destination"/> is invalid. This includes paths with invalid characters, empty strings, or relative paths.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if the source directory does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permissions.</exception>
    /// <exception cref="IOException">Thrown if an I/O error occurs during compression.</exception>
    public static async Task CreateFromDirectoryAsync(IFileSystem fileSystemReference, string source,
        string destination)
    {
        ValidatePath(source);
        ValidatePath(destination);
        using var archive = GetWritableZipArchive(fileSystemReference, destination);

        var files = GetAllFilesExceptSymbolicLinksToPreventLoops(fileSystemReference, source);
        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(source, file);
            await AddFileToArchiveAsync(fileSystemReference, archive, relativePath, file);
        }
    }

    /// <summary>
    /// Validates the path to ensure that it is not null or empty, does not contain any invalid characters and is absolute.
    /// </summary>
    /// <param name="path">The path to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="path"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="path"/> is empty, contains invalid characters or is not absolute.</exception>
    private static void ValidatePath(string path)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path), "Path cannot be null.");

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("The path cannot be empty or whitespace.", nameof(path));
        }

        if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            throw new ArgumentException($"The path contains invalid characters: {path}", nameof(path));
        }

        if (!Path.IsPathFullyQualified(path))
        {
            throw new ArgumentException($"The path must be absolute: {path}", nameof(path));
        }
    }

    private static async Task AddFileToArchiveAsync(IFileSystem fileSystemReference, ZipArchive archive,
        string relativePath, string file)
    {
        var entry = archive.CreateEntry(relativePath);
        await using var entryStream = entry.Open();
        await using var fileStream = fileSystemReference.File.OpenRead(file);
        await fileStream.CopyToAsync(entryStream);
    }

    private static string[] GetAllFilesExceptSymbolicLinksToPreventLoops(IFileSystem fileSystemReference, string source)
    {
        return fileSystemReference.Directory.GetFiles(source, "*", SearchOption.AllDirectories)
            .Where(file => !fileSystemReference.File.GetAttributes(file).HasFlag(FileAttributes.ReparsePoint))
            .ToArray();
    }


    private static void CreateEntryDirectory(IFileSystem fileSystemReference, string fullPath)
    {
        var directoryName = Path.GetDirectoryName(fullPath);
        if (directoryName != null) fileSystemReference.Directory.CreateDirectory(directoryName);
    }

    private static string NormalizeExtractedEntryPathForWindowsAndUnix(ZipArchiveEntry entry)
    {
        return Path.DirectorySeparatorChar switch
        {
            //adjust paths for unpacking on unix when packed on windows
            '/' => entry.FullName.Replace("\\", "/"),
            //adjust paths for unpacking on windows when packed on unix
            '\\' => entry.FullName.Replace("/", "\\"),
            _ => entry.FullName
        };
    }

    private static void EnsureDestinationDirectoryExists(IFileSystem fileSystemReference, string destination)
    {
        if (!fileSystemReference.Directory.Exists(destination))
        {
            fileSystemReference.Directory.CreateDirectory(destination);
        }
    }
}