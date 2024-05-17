using System.IO.Abstractions;
using System.IO.Compression;
using DataAccess.Extensions;
using DataAccessTest.Resources;
using NUnit.Framework;

namespace DataAccessTest.Extensions;

[TestFixture]
public class ZipExtensionsUt
{
    private IFileSystem _fileSystem;

    [SetUp]
    public void Setup()
    {
        _fileSystem = ResourceHelper.PrepareFileSystemWithResources();
    }

    [Test]
    public void ExtractToDirectory_ExtractsProperlyIntoMockFs()
    {
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
}