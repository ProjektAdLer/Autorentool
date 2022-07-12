using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.sections;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
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
        sectionSection.SetParameters("h5pElementId", "$@NULL@$",
            "$@NULL@$", "0", "$@NULL@$", "1", 
            "$@NULL@$", "currentTime", "h5pElementId");

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sectionSection.Id, Is.EqualTo("h5pElementId"));
            Assert.That(sectionSection.Number, Is.EqualTo("h5pElementId"));

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
        systemUnderTest.SetParameters("h5pElementId", "$@NULL@$",
            "$@NULL@$", "0", "$@NULL@$", "1", 
            "$@NULL@$", "currentTime", "h5pElementId");
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;

        //Act
        systemUnderTest.Serialize("", "1");
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "sections", "section_1", "section.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}