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
            var path = Path.Combine(destination, entry.FullName);
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName != null) fs.Directory.CreateDirectory(directoryName);
            if (fs.File.Exists(path)) fs.File.Delete(path);

            using var destStream = fs.File.Create(path);
            using var sourceStream = entry.Open();
            sourceStream.CopyTo(destStream);
        }
    }
}