using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.GradeHistory.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Grades.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Module.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Resource.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Roles.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Section.xml;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using Microsoft.VisualBasic.FileIO;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class XmlFileFactoryUt
{
    [Test]
    public void XmFileFactory_Constructor_AllParametersSet()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var fileString = "fileString";
        var mockFileManager = new XmlFileManager();
        var mockFileSystem = new MockFileSystem();

        // Act
        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, mockFileManager, mockFileSystem);

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(systemUnderTest._currentTime, Is.Not.Empty);
            Assert.That(systemUnderTest._fileManager, Is.EqualTo(mockFileManager));
            Assert.That(systemUnderTest._fileSystem, Is.EqualTo(mockFileSystem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlActivityGradebook, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlResource, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesRolesXmlRoles, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileBlock1, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileBlock2, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.Not.Null);
            Assert.That(systemUnderTest.SectionsInforefXmlInforef, Is.Not.Null);
            Assert.That(systemUnderTest.SectionsSectionXmlSection, Is.Not.Null);
            
        });
    }

    [Test]
    public void XmlFileFactory_CreateFileFactory_FileListCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var identifier = new IdentifierJson();
        identifier.type = "FileName";    
        identifier.value = "DSL_Document";

        var dslDocument = new LearningElementJson();
        dslDocument.id = 1;
        dslDocument.identifier = identifier;
        dslDocument.elementType = "json";

        var dslDocumentList = new List<LearningElementJson>()
        {
            dslDocument
        };
        
        mockReadDsl.GetDslDocumentList().Returns(dslDocumentList);
        var fileString = Path.Join(currWorkDir, "XMLFilesForExport", identifier.value, "Hello World");
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport", identifier.value), new MockFileData("Hello World"));

        
        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, mockFileManager, mockFileSystem);
        systemUnderTest.CreateFileFactory();
        
        
        // Assert
        Assert.Multiple(() =>
        {
            systemUnderTest._fileManager.Received().SetXmlFilesList(systemUnderTest.filesXmlFilesList);
            systemUnderTest._fileManager.Received().CalculateHashCheckSumAndFileSize(fileString);
            systemUnderTest._fileManager.Received().CreateFolderAndFiles(fileString, systemUnderTest._fileManager.GetHashCheckSum());
            systemUnderTest._fileManager.Received().GetFileSize();
            systemUnderTest._fileManager.Received().GetHashCheckSum();
            
        });
    }

    [Test]
    public void FileSetParametersFilesXml_SetFileXmlEntity_AndAddToFilesList()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        // Act

        var systemUnderTest = new XmlFileFactory(mockReadDsl, "", mockFileManager, mockFileSystem);
        systemUnderTest.filesXmlFilesList = new List<FilesXmlFile>();
        systemUnderTest.fileElementId = "1";
        systemUnderTest.fileElementName = "FileName";
        systemUnderTest.FileSetParametersFilesXml("1234", "456789");
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.filesXmlFilesList[0].ContentHash, Is.EqualTo("1234"));
            Assert.That(systemUnderTest.filesXmlFilesList[0].ContextId, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.filesXmlFilesList[0].Filename, Is.EqualTo(systemUnderTest.fileElementName));
            Assert.That(systemUnderTest.filesXmlFilesList[0].Filesize, Is.EqualTo("456789"));
            Assert.That(systemUnderTest.filesXmlFilesList[0].Source, Is.EqualTo(systemUnderTest.fileElementName));
            Assert.That(systemUnderTest.filesXmlFilesList[0].Timecreated, Is.EqualTo(systemUnderTest._currentTime));
            Assert.That(systemUnderTest.filesXmlFilesList[0].Timemodified, Is.EqualTo(systemUnderTest._currentTime));
            Assert.That(systemUnderTest.filesXmlFilesList[1].ContentHash, Is.EqualTo("1234"));
            Assert.That(systemUnderTest.filesXmlFilesList[1].ContextId, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.filesXmlFilesList[1].Filename, Is.EqualTo(systemUnderTest.fileElementName));
            Assert.That(systemUnderTest.filesXmlFilesList[1].Filesize, Is.EqualTo("456789"));
            Assert.That(systemUnderTest.filesXmlFilesList[1].Source, Is.EqualTo(systemUnderTest.fileElementName));
            Assert.That(systemUnderTest.filesXmlFilesList[1].Timecreated, Is.EqualTo(systemUnderTest._currentTime));
            Assert.That(systemUnderTest.filesXmlFilesList[1].Timemodified, Is.EqualTo(systemUnderTest._currentTime));
            Assert.That(systemUnderTest.filesXmlFilesList, Has.Count.EqualTo(2));
        });
    }
    
    [Test]
    public void FileSetParametersFilesXml_ListIsNull()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        // Act

        var systemUnderTest = new XmlFileFactory(mockReadDsl, "", mockFileManager, mockFileSystem);
        systemUnderTest.fileElementId = "1";
        systemUnderTest.fileElementName = "FileName";
        systemUnderTest.FileSetParametersFilesXml("1234", "456789");
        
        // Assert
        Assert.That(systemUnderTest.filesXmlFilesList, Is.Null);
    }

    [Test]
    public void FileSetParametersActivity_CreateActivityFolder_SetsGradesResourceRolesModuleGradeHistoryInforef_AndSerializes()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var mockGradesGradeItem = new ActivitiesGradesXmlGradeItem();
        var mockGradesGradeItems = new ActivitiesGradesXmlGradeItems();
        var mockGradesGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        var mockFileResource = new ActivitiesResourceXmlResource();
        var mockFileResourceActivity = Substitute.For<IActivitiesResourceXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        var mockGradehistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();
        var mockInforefFile = new ActivitiesInforefXmlFile();
        var mockInforefFileref = new ActivitiesInforefXmlFileref();
        var mockInforefGradeItem = new ActivitiesInforefXmlGradeItem();
        var mockInforefGradeItemref = new ActivitiesInforefXmlGradeItemref();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();

        // Act
        var systemUnderTest = new XmlFileFactory(mockReadDsl, "", mockFileManager, mockFileSystem, mockGradesGradeItem, 
            mockGradesGradeItems, mockGradesGradebook, mockFileResource, mockFileResourceActivity, mockRoles, mockModule,
            mockGradehistory, mockInforefFile, mockInforefFileref, mockInforefGradeItem, mockInforefGradeItemref, mockInforefInforef);
        systemUnderTest.fileElementId = "1";
        systemUnderTest.FileSetParametersActivity();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradesGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradesGradeItems));
            systemUnderTest.ActivitiesGradesXmlActivityGradebook.Received().Serialize("resource", systemUnderTest.fileElementId);
            
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlResource, Is.EqualTo(mockFileResource));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.Id, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ModuleId, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ModuleName, Is.EqualTo( "resource"));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ContextId, Is.EqualTo(systemUnderTest.fileElementId));
            systemUnderTest.ActivitiesFileResourceXmlActivity.Received().Serialize("resource", systemUnderTest.fileElementId);
            
            systemUnderTest.ActivitiesRolesXmlRoles.Received().Serialize("resource", systemUnderTest.fileElementId);
            
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ModuleName, Is.EqualTo("resource"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionId, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionNumber, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Added, Is.EqualTo(systemUnderTest._currentTime));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Id, Is.EqualTo(systemUnderTest.fileElementId));
            systemUnderTest.ActivitiesModuleXmlModule.Received().Serialize("resource", systemUnderTest.fileElementId);
            
            systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory.Received().Serialize("resource", systemUnderTest.fileElementId);
            
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref.File, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref.GradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef.Fileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef.GradeItemref, Is.EqualTo(mockInforefGradeItemref));
            systemUnderTest.ActivitiesInforefXmlInforef.Received().Serialize("resource", systemUnderTest.fileElementId);
        });
    }

    [Test]
    public void FileSetParametersSections_CreateSectionFolder_SetsInforefSection_AndSerializes()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        var mockSection = Substitute.For<ISectionsSectionXmlSection>();
        var mockInforefSection = Substitute.For<ISectionsInforefXmlInforef>();

        // Act
        var systemUnderTest = new XmlFileFactory(mockReadDsl, "", mockFileManager, mockFileSystem, sectionsInforefXmlInforef: mockInforefSection,
            sectionsSectionXmlSection: mockSection);
        systemUnderTest.fileElementId = "1";
        systemUnderTest.FileSetParametersSections();
        
        // Assert
        Assert.Multiple(() =>
        {
            systemUnderTest.SectionsInforefXmlInforef.Received().Serialize("",  systemUnderTest.fileElementId);
            
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Number, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Id, Is.EqualTo(systemUnderTest.fileElementId));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Name, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Summary, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Timemodified, Is.EqualTo(systemUnderTest._currentTime));
            systemUnderTest.SectionsSectionXmlSection.Received().Serialize("",  systemUnderTest.fileElementId);
            
        });
        
    }
    

    [Test]
    public void CreateActivityFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var fileString = "";
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, fileSystem: mockFileSystem);
        
        //Act
        systemUnderTest.CreateActivityFolder("1");
        
        //Assert
        Assert.That(mockFileSystem.Directory.Exists(Path.Join("XMLFilesForExport", "activities", "resource_"+"1")), Is.True);
    }
    
    [Test]
    public void CreateSectionFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var fileString = "";
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, fileSystem: mockFileSystem);
        
        //Act
        systemUnderTest.CreateSectionsFolder("1");
        
        //Assert
        Assert.That(mockFileSystem.Directory.Exists(Path.Join("XMLFilesForExport", "sections", "section_"+"1")), Is.True);
    }
}