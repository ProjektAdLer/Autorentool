using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Runtime.Serialization;
using DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessTest.Persistence;

[TestFixture]
public class XmlFileHandlerUt
{
    private class TestNotSerializable
    {
        public TestNotSerializable()
        {
            Name = "hello";
        }
        //Disable warning for test
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string Name { get; set; }
    }

    [Serializable]
    private class TestNoParameterlessConstructor
    {
        public TestNoParameterlessConstructor(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    [Serializable]
    public class TestSerializable
    {
        public TestSerializable()
        {
            Name = "";
            Number = 0;
        }
        public TestSerializable(string name, int number)
        {
            Name = name;
            Number = number;
        }
        
        public string Name { get; set; }
        public int Number { get; set; }
    }
    
    [Test]
    public void XmlFileHandler_Constructor_FailsIfTypeNotSerializable()
    {
        //Disable warning for test
        // ReSharper disable once ObjectCreationAsStatement
        var ex = Assert.Throws<InvalidOperationException>(() => { new XmlFileHandler<TestNotSerializable>(null!); });
        Assert.That(ex!.Message, Is.EqualTo($"Type {nameof(TestNotSerializable)} is not serializable."));
    }
    
    [Test]
    public void XmlFileHandler_Constructor_FailsIfTypeDoesNotHaveParameterlessConstructor()
    {
        //Disable warning for test
        // ReSharper disable once ObjectCreationAsStatement
        var ex = Assert.Throws<InvalidOperationException>(() => { new XmlFileHandler<TestNoParameterlessConstructor>(null!); });
        Assert.That(ex!.Message, Is.EqualTo($"Type {nameof(TestNoParameterlessConstructor)} has no required parameterless constructor."));
    }
    
    [Test]
    public void XmlFileHandler_SaveToDisk_CreatesCorrectFile()
    {
        var obj = new TestSerializable("foo", 123);
        var mockFileSystem = new MockFileSystem();
        const string filepath = "foobar.txt";
        
        var systemUnderTest = CreateTestableXmlFileHandler<TestSerializable>(fileSystem: mockFileSystem);
        
        systemUnderTest.SaveToDisk(obj, filepath);
        
        var file = mockFileSystem.GetFile(filepath);
        Assert.That(file.TextContents, Is.EqualTo("<?xml version=\"1.0\" encoding=\"utf-8\"?><TestSerializable xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Name>foo</Name><Number>123</Number></TestSerializable>"));
    }

    [Test]
    public void XmlFileHandler_LoadFromDisk_CreatesCorrectObject()
    {
        var mockFileSystem = new MockFileSystem();
        const string filepath = "foobar.txt";
        mockFileSystem.AddFile(filepath, "<?xml version=\"1.0\" encoding=\"utf-8\"?><TestSerializable xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Name>foo</Name><Number>123</Number></TestSerializable>");
        
        var systemUnderTest = CreateTestableXmlFileHandler<TestSerializable>(fileSystem: mockFileSystem);

        var objActual = systemUnderTest.LoadFromDisk(filepath);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foo"));
            Assert.That(objActual.Number, Is.EqualTo(123));
        });
    }

    [Test]
    public void XmlFileHandler_SaveToDisk_ThrowsWrappedException()
    {
        var mockFileSystem = new MockFileSystem();
        const string filepath = "foobar.txt";
        var mockFileData = new MockFileData("pop");
        //create read only file
        mockFileData.Attributes = FileAttributes.ReadOnly;
        mockFileSystem.AddFile(filepath, mockFileData);
        var obj = new TestSerializable("foo", 123);

        var systemUnderTest = CreateTestableXmlFileHandler<TestSerializable>(fileSystem: mockFileSystem);

        var ex = Assert.Throws<SerializationException>(() => systemUnderTest.SaveToDisk(obj, filepath));
        var innerEx = ex!.InnerException;
        Assert.Multiple(() =>
        {
            Assert.That(ex.Message, Is.EqualTo($"Couldn't serialize TestSerializable object into file at {filepath}."));
            Assert.That(innerEx, Is.Not.Null);
        });
        Assert.That(innerEx!.GetType(), Is.EqualTo(typeof(UnauthorizedAccessException)));
    }

    [Test]
    public void XmlFileHandler_LoadFromDisk_ThrowsWrappedException()
    {
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableXmlFileHandler<TestSerializable>(fileSystem: mockFileSystem);

        var ex = Assert.Throws<SerializationException>(() => systemUnderTest.LoadFromDisk("foo"));
        var innerEx = ex!.InnerException;
        Assert.Multiple(() =>
        {
            Assert.That(ex.Message, Is.EqualTo("Couldn't deserialize file at foo into TestSerializable object."));
            Assert.That(innerEx, Is.Not.Null);
        });
        Assert.That(innerEx!.GetType(), Is.EqualTo(typeof(FileNotFoundException)));
    }

    private XmlFileHandler<T> CreateTestableXmlFileHandler<T>(ILogger<XmlFileHandler<T>>? logger = null, IFileSystem? fileSystem = null) where T : class
    {
        logger ??= Substitute.For<ILogger<XmlFileHandler<T>>>();
        return fileSystem == null ? new XmlFileHandler<T>(logger) : new XmlFileHandler<T>(logger, fileSystem);
    }
}