using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.sections;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using Castle.Components.DictionaryAdapter;
using NSubstitute.Core.Arguments;
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
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        ReadDSL? dsl = new ReadDSL();
        var h5pfactory = new XmlH5PFactory(dsl, mockFileSystem, null, null, null,null,
            null,null,null,null,null,null,null,
            null,null,null,null,
            null,null);

        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var sectionSection = new SectionsSectionXmlSection();
        sectionSection.SetParameters("h5pElementId", "$@NULL@$",
            "$@NULL@$", "0", "$@NULL@$", "1", 
            "$@NULL@$", "currentTime", "h5pElementId");

        //Act
        h5pfactory.CreateSectionsFolder("1");
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        sectionSection.Serialize("1");
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "sections", "section_1", "section.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}