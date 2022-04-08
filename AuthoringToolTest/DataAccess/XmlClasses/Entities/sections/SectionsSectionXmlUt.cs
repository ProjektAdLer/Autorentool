using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.sections;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.sections;

[TestFixture]
public class SectionsSectionXmlUt
{
    [Test]
    public void SectionsSectionXmlSection_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var sectionSection = new SectionsSectionXmlSection();

        //Act
        sectionSection.SetParameters("160", "1");

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sectionSection.Id, Is.EqualTo("160"));
            Assert.That(sectionSection.Number, Is.EqualTo("1"));

        });
    }

    [Test]

    public void SectionsSectionXmlSection_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var sectionSection = new SectionsSectionXmlSection();
        sectionSection.SetParameters("160","1");

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        sectionSection.Serialize();
        
        //Assert
        var pathXml = Path.Join(curWorkDir, "XMLFilesForExport");
        var pathXmlPartOne = Path.Join(pathXml, "sections");
        var pathXmlPartTwo = Path.Join(pathXmlPartOne, "section_160");
        var pathXmlFile = Path.Join(pathXmlPartTwo, "section.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}