using System.IO.Abstractions;
using System.IO.Compression;

namespace DataAccess.Extensions;

public static class ZipExtensions
{
    public static ZipArchive GetZipArchive(IFileSystem fs, string archivePath)
    {
        var zipStream = fs.File.OpenRead(archivePath);
        return new ZipArchive(zipStream, ZipArchiveMode.Read);
    }
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