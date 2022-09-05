using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Generator.WorldExport;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Resource.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;
using Generator.XmlClasses.Entities.Files.xml;
using Generator.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.XmlFileFactories;

[TestFixture]
public class XmlFileFactoryUt
{
    [Test]
    public void XmFileFactory_Constructor_AllParametersSet()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var fileString = "fileString";
        var mockFileManager = new XmlFileManager();
        var mockFileSystem = new MockFileSystem();

        // Act
        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, mockFileManager, mockFileSystem);

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(systemUnderTest.CurrentTime, Is.Not.Empty);
            Assert.That(systemUnderTest.FileManager, Is.EqualTo(mockFileManager));
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
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var identifier = new IdentifierJson("FileName", "DSL_Document");

        var dslDocument = new LearningElementJson(1, identifier, "json",0);

        var dslDocumentList = new List<LearningElementJson>()
        {
            dslDocument
        };
        
        mockReadDsl.GetDslDocumentList().Returns(dslDocumentList);
        var fileString = Path.Join(currWorkDir, "XMLFilesForExport", identifier.Value, "Hello World");
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport", identifier.Value), new MockFileData("Hello World"));

        
        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, mockFileManager, mockFileSystem);
        systemUnderTest.CreateFileFactory();
        
        
        // Assert
        Assert.Multiple(() =>
        {
            systemUnderTest.FileManager.Received().SetXmlFilesList(systemUnderTest.FilesXmlFilesList);
            systemUnderTest.FileManager.Received().CalculateHashCheckSumAndFileSize(fileString);
            systemUnderTest.FileManager.Received().CreateFolderAndFiles(fileString, systemUnderTest.FileManager.GetHashCheckSum());
            systemUnderTest.FileManager.Received().GetFileSize();
            systemUnderTest.FileManager.Received().GetHashCheckSum();
            
        });
    }

    [Test]
    public void FileSetParametersFilesXml_SetFileXmlEntity_AndAddToFilesList()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        // Act

        var systemUnderTest = new XmlFileFactory(mockReadDsl, "", mockFileManager, mockFileSystem);
        systemUnderTest.FilesXmlFilesList = new List<FilesXmlFile>();
        systemUnderTest.FileElementId = "1";
        systemUnderTest.FileElementName = "FileName";
        systemUnderTest.FileSetParametersFilesXml("1234", "456789");
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FilesXmlFilesList[0].ContentHash, Is.EqualTo("1234"));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].ContextId, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Filename, Is.EqualTo(systemUnderTest.FileElementName));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Filesize, Is.EqualTo("456789"));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Source, Is.EqualTo(systemUnderTest.FileElementName));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Timecreated, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Timemodified, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].ContentHash, Is.EqualTo("1234"));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].ContextId, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Filename, Is.EqualTo(systemUnderTest.FileElementName));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Filesize, Is.EqualTo("456789"));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Source, Is.EqualTo(systemUnderTest.FileElementName));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Timecreated, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Timemodified, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.FilesXmlFilesList, Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void FileSetParametersActivity_CreateActivityFolder_SetsGradesResourceRolesModuleGradeHistoryInforef_AndSerializes()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
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
            mockGradesGradeItems, mockGradesGradebook, mockFileResource, mockFileResourceActivity, mockRoles,
            mockModule, mockGradehistory, mockInforefFile, mockInforefFileref, mockInforefGradeItem, mockInforefGradeItemref,
            mockInforefInforef);

        systemUnderTest.FileSetParametersActivity();
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradesGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradesGradeItems));
            systemUnderTest.ActivitiesGradesXmlActivityGradebook.Received().Serialize("resource", systemUnderTest.FileElementId);
            
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlResource, Is.EqualTo(mockFileResource));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.Id, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ModuleId, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ModuleName, Is.EqualTo( "resource"));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ContextId, Is.EqualTo(systemUnderTest.FileElementId));
            systemUnderTest.ActivitiesFileResourceXmlActivity.Received().Serialize("resource", systemUnderTest.FileElementId);
            
            systemUnderTest.ActivitiesRolesXmlRoles.Received().Serialize("resource", systemUnderTest.FileElementId);
            
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ModuleName, Is.EqualTo("resource"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionId, Is.EqualTo(systemUnderTest.FileElementParentSpace));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionNumber, Is.EqualTo(systemUnderTest.FileElementParentSpace));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Added, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Id, Is.EqualTo(systemUnderTest.FileElementId));
            systemUnderTest.ActivitiesModuleXmlModule.Received().Serialize("resource", systemUnderTest.FileElementId);
            
            systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory.Received().Serialize("resource", systemUnderTest.FileElementId);
            
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref.File, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref.GradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef.Fileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef.GradeItemref, Is.EqualTo(mockInforefGradeItemref));
            systemUnderTest.ActivitiesInforefXmlInforef.Received().Serialize("resource", systemUnderTest.FileElementId);
        });
    }

    /*[Test]
    public void FileSetParametersSections_CreateSectionFolder_SetsInforefSection_AndSerializes()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        var mockSection = Substitute.For<ISectionsSectionXmlSection>();
        var mockInforefSection = Substitute.For<ISectionsInforefXmlInforef>();

        // Act
        var systemUnderTest = new XmlFileFactory(mockReadDsl, "", mockFileManager, mockFileSystem, sectionsInforefXmlInforef: mockInforefSection,
            sectionsSectionXmlSection: mockSection);
        systemUnderTest.FileElementId = "1";
        systemUnderTest.FileSetParametersSections();
        
        // Assert
        Assert.Multiple(() =>
        {
            systemUnderTest.SectionsInforefXmlInforef.Received().Serialize("",  systemUnderTest.FileElementId);
            
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Number, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Id, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Name, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Summary, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Timemodified, Is.EqualTo(systemUnderTest.CurrentTime));
            systemUnderTest.SectionsSectionXmlSection.Received().Serialize("",  systemUnderTest.FileElementId);
            
        });
        
    }*/
    

    [Test]
    public void CreateActivityFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var fileString = "";
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, fileSystem: mockFileSystem);
        
        //Act
        systemUnderTest.CreateActivityFolder("1");
        
        //Assert
        Assert.That(mockFileSystem.Directory.Exists(Path.Join("XMLFilesForExport", "activities", "resource_"+"1")), Is.True);
    }
    
    /*
    [Test]
    public void CreateSectionFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var fileString = "";
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlFileFactory(mockReadDsl, fileString, fileSystem: mockFileSystem);
        
        //Act
        systemUnderTest.CreateSectionsFolder("1");
        
        //Assert
        Assert.That(mockFileSystem.Directory.Exists(Path.Join("XMLFilesForExport", "sections", "section_"+"1")), Is.True);
    }*/
}