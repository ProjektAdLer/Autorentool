using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Scales.xml;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities.Scales.xml;

[TestFixture]
public class ScalesXmlUt
{
    [Test]
    // ANF-ID: [GHO11]
    public void ScalesXmlScalesDefinition_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Combine(curWorkDir, "XMLFilesForExport"));

        var systemUnderTest = new ScalesXmlScalesDefinition();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();

        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "scales.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}