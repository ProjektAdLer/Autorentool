using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.sections;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.sections;

[TestFixture]
public class SectionsSectionXmlUt
{
    [Test]
    public void SectionsSectionXmlSection_SetParameters_ObjectsAreEqual()
    {
        //Arrange

        //Act
        var systemUnderTest = new SectionsSectionXmlSection();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Number, Is.EqualTo(""));
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Summary, Is.EqualTo(""));
            Assert.That(systemUnderTest.SummaryFormat, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Sequence, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Visible, Is.EqualTo("1"));
            Assert.That(systemUnderTest.AvailabilityJson, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
    }

    [Test]
    public void SectionsSectionXmlSection_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();

        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(curWorkDir, "XMLFilesForExport", "sections", "section_1"));
        
        var systemUnderTest = new SectionsSectionXmlSection();

        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;

        //Act
        systemUnderTest.Serialize("", "1");
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "sections", "section_1", "section.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}