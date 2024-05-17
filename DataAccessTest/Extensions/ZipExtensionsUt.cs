using System.IO.Abstractions;
using System.IO.Compression;
using DataAccess.Extensions;
using DataAccessTest.Resources;
using NUnit.Framework;

namespace DataAccessTest.Extensions;

[TestFixture]
public class ZipExtensionsUt
{
    [SetUp]
    public void Setup()
    {
    }

    private IFileSystem _fileSystem;

    [Test]
    [Platform(Exclude = "Linux, Unix, MacOSX")]
    public void ExtractToDirectory_ExtractsProperlyIntoMockFs_Windows()
    {
        _fileSystem = ResourceHelper.PrepareWindowsFileSystemWithResources();
        using var zipStream = _fileSystem.File.OpenRead("C:\\zips\\import_test.zip");
        var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        zipArchive.ExtractToDirectory(_fileSystem, "C:\\somerandomdir");
        Assert.Multiple(() =>
        {
            Assert.That(_fileSystem.Directory.Exists("C:\\somerandomdir"));
            Assert.That(_fileSystem.Directory.Exists("C:\\somerandomdir\\Content"));
            Assert.That(_fileSystem.File.Exists("C:\\somerandomdir\\world.awf"));
            Assert.That(_fileSystem.File.Exists("C:\\somerandomdir\\Content\\regex.txt"));
            Assert.That(_fileSystem.File.Exists("C:\\somerandomdir\\Content\\regex.txt.hash"));
            Assert.That(_fileSystem.File.Exists("C:\\somerandomdir\\Content\\adler_logo.png"));
            Assert.That(_fileSystem.File.Exists("C:\\somerandomdir\\Content\\adler_logo.png.hash"));
            Assert.That(_fileSystem.File.Exists("C:\\somerandomdir\\Content\\.linkstore"));
        });
    }

    [Test]
    [Platform(Include = "Linux, Unix, MacOSX")]
    public void ExtractToDirectory_ExtractsProperlyIntoMockFs_Unix()
    {
        _fileSystem = ResourceHelper.PrepareUnixFileSystemWithResources();
        using var zipStream = _fileSystem.File.OpenRead("/zips/import_test.zip");
        var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);

        zipArchive.ExtractToDirectory(_fileSystem, "/somerandomdir");
        Assert.Multiple(() =>
        {
            Assert.That(_fileSystem.Directory.Exists("/somerandomdir"));
            Assert.That(_fileSystem.Directory.Exists("/somerandomdir/Content"));
            Assert.That(_fileSystem.File.Exists("/somerandomdir/world.awf"));
            Assert.That(_fileSystem.File.Exists("/somerandomdir/Content/regex.txt"));
            Assert.That(_fileSystem.File.Exists("/somerandomdir/Content/regex.txt.hash"));
            Assert.That(_fileSystem.File.Exists("/somerandomdir/Content/adler_logo.png"));
            Assert.That(_fileSystem.File.Exists("/somerandomdir/Content/adler_logo.png.hash"));
            Assert.That(_fileSystem.File.Exists("/somerandomdir/Content/.linkstore"));
        });
    }
}