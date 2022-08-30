using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace PresentationTest.DataAccess.Persistence;

[TestFixture]

public class ContentFileHandlerUt
{
    [Test]
    public void ContentFileHandler_LoadFromDisk_CreatesCorrectObject()
    {
        const string filepath = "foobar.png";
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54})},
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: mockFileSystem);

        var objActual = systemUnderTest.LoadFromDisk(filepath);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar.png"));
            Assert.That(objActual.Type, Is.EqualTo("png"));
            Assert.That(objActual.Content, Is.EqualTo(new byte[] {0x42, 0x24, 0x53, 0x54}));
        });
    }
    
    [Test]
    public void ContentFileHandler_LoadFromStream_CreatesCorrectObject()
    {
        const string filepath = "foobar.png";
        var stream = new MemoryStream(new byte[] {0x42, 0x24, 0x53, 0x54});
        

        var systemUnderTest = CreateTestableContentFileHandler();

        var objActual = systemUnderTest.LoadFromStream(filepath, stream);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar.png"));
            Assert.That(objActual.Type, Is.EqualTo("png"));
            Assert.That(objActual.Content, Is.EqualTo(new byte[] {0x42, 0x24, 0x53, 0x54}));
        });
    }

    private ContentFileHandler CreateTestableContentFileHandler(ILogger<ContentFileHandler>? logger = null,
        IFileSystem? fileSystem = null)
    {
        logger ??= Substitute.For<ILogger<ContentFileHandler>>();
        return fileSystem == null ? new ContentFileHandler(logger) : new ContentFileHandler(logger, fileSystem);
    }
}