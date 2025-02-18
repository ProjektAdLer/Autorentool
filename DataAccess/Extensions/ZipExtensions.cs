using System.IO.Abstractions;
using System.IO.Compression;


namespace DataAccess.Extensions;

public static class ZipExtensions
{
    /// <summary>
    /// Creates a <see cref="ZipArchive"/> object for a given path on the filesystem <paramref name="fs"/>.
    /// </summary>
    /// <param name="fs">The filesystem on which the file resides.</param>
    /// <param name="archivePath">The path to the zip archive on the filesystem.</param>
    /// <returns></returns>
    public static ZipArchive GetZipArchive(IFileSystem fs, string archivePath)
    {
        var zipStream = fs.File.OpenRead(archivePath);
        return new ZipArchive(zipStream, ZipArchiveMode.Read);
    }

    private static ZipArchive GetWritableZipArchive(IFileSystem fs, string archivePath)
    {
        var zipStream = fs.File.OpenWrite(archivePath);
        return new ZipArchive(zipStream, ZipArchiveMode.Create);
    }

    /// <summary>
    /// Extracts the contents of a <see cref="ZipArchive"/> to a given directory on the filesystem <paramref name="fs"/>.
    /// </summary>
    /// <param name="archive">The zip archive to extract.</param>
    /// <param name="fs">The filesystem to extract the archive onto.</param>
    /// <param name="destination">The destination path on the filesystem.</param>
    public static void ExtractToDirectory(this ZipArchive archive, IFileSystem fs, string destination)
    {
        if (!fs.Directory.Exists(destination))
        {
            fs.Directory.CreateDirectory(destination);
        }

        foreach (var entry in archive.Entries)
        {
            var entryFullName = Path.DirectorySeparatorChar switch
            {
                //adjust paths for unpacking on unix when packed on windows
                '/' => entry.FullName.Replace("\\", "/"),
                //adjust paths for unpacking on windows when packed on unix
                '\\' => entry.FullName.Replace("/", "\\"),
                _ => entry.FullName
            };
            var path = Path.Combine(destination, entryFullName);
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName != null) fs.Directory.CreateDirectory(directoryName);
            if (fs.File.Exists(path)) fs.File.Delete(path);

            using var destStream = fs.File.Create(path);
            using var sourceStream = entry.Open();
            sourceStream.CopyTo(destStream);
        }
    }

    /// <summary>
    /// Creates a zip archive from a given directory on the filesystem <paramref name="fs"/>.
    /// </summary>
    /// <param name="fs">The filesystem to operate on.</param>
    /// <param name="source">The folder that should be packed into the zip archive.</param>
    /// <param name="destination">The file path the zip archive should be written to.</param>
    public static async Task CreateFromDirectoryAsync(IFileSystem fs, string source, string destination)
    {
        using var archive = GetWritableZipArchive(fs, destination);
        var files = fs.Directory.GetFiles(source, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(source, file);
            var entry = archive.CreateEntry(relativePath);
            await using var entryStream = entry.Open();
            await using var fileStream = fs.File.OpenRead(file);
            await fileStream.CopyToAsync(entryStream);
        }
    }
}