using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities._sections;

[TestFixture]
public class SectionsInforefXmlInforefUt
{
    [Test]
    // ANF-ID: [GHO11]
    public void SectionsInforefXmlInforef_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Join(curWorkDir, "XMLFilesForExport", "sections", "section_1"));

        var systemUnderTest = new SectionsInforefXmlInforef();
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;

        //Act
        systemUnderTest.Serialize("", "1");

        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "sections", "section_1", "inforef.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}