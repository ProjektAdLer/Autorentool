using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Outcomes.xml;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities.Outcomes.xml;

[TestFixture]
public class OutcomesXmlOutcomesDefinitionUt
{
    [Test]
    // ANF-ID: [GHO11]
    public void OutcomesXmlOutcomesDefinition_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Combine(curWorkDir, "XMLFilesForExport"));

        var systemUnderTest = new OutcomesXmlOutcomesDefinition();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();

        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "outcomes.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}