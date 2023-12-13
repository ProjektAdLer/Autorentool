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

        _fileSystem.CreateDisposableDirectory(out var dir);
        
        zipArchive.ExtractToDirectory(_fileSystem, dir.FullName);
    }
}