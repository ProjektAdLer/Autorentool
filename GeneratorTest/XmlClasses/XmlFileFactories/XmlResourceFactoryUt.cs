using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
using Generator.WorldExport;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Resource.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities.Files.xml;
using Generator.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.XmlFileFactories;

[TestFixture]
public class XmlResourceFactoryUt
{
    [Test]
    public void XmlResourceFactory_Constructor_AllParametersSet()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileManager = new XmlFileManager();
        var mockFileSystem = new MockFileSystem();

        // Act
        var systemUnderTest = new XmlResourceFactory(mockReadAtf, mockFileManager, mockFileSystem);

        // Assert
        Assert.Multiple(() =>
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
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void XmlResourceFactory_CreateFileFactory_FileListCreated()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var currWorkDir = ApplicationPaths.BackupFolder;

        var learningEvl = 4;

        var jsonDocument = new LearningElementJson(1, "", "Document", "", "", "json", 1, learningEvl, "");
        var pngDocument = new LearningElementJson(2, "", "Document", "", "", "png", 1, learningEvl, "");
        var mp4Document = new LearningElementJson(3, "", "Document", "", "", "mp4", 1, learningEvl, "");
        var webpDocument = new LearningElementJson(4, "", "Document", "", "", "webp", 1, learningEvl, "");
        var jsDocument = new LearningElementJson(5, "", "Document", "", "", "js", 1, learningEvl, "");
        var cssDocument = new LearningElementJson(6, "", "Document", "", "", "css", 1, learningEvl, "");
        var htmlDocument = new LearningElementJson(7, "", "Document", "", "", "html", 1, learningEvl, "");
        var csDocument = new LearningElementJson(8, "", "Document", "", "", "cs", 1, learningEvl, "");
        var ccDocument = new LearningElementJson(9, "", "Document", "", "", "cc", 1, learningEvl, "");
        var cPlusPlusDocument = new LearningElementJson(10, "", "Document", "", "", "cpp", 1, learningEvl, "");
        var txtDocument = new LearningElementJson(11, "", "Document", "", "", "txt", 1, learningEvl, "");


        var resourceList = new List<ILearningElementJson>
        {
            jsonDocument,
            pngDocument,
            mp4Document,
            webpDocument,
            jsDocument,
            cssDocument,
            htmlDocument,
            csDocument,
            ccDocument,
            cPlusPlusDocument,
            txtDocument
        };

        mockReadAtf.GetResourceElementList().Returns(resourceList);
        var space_1 = new LearningSpaceJson(1, "", "space", new List<int?> { 1, 2 }, 10, "", "");
        var fileString = Path.Join(currWorkDir, "XMLFilesForExport", "Document");
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport", "Document"),
            new MockFileData("Hello World"));


        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        var systemUnderTest = new XmlResourceFactory(mockReadAtf, mockFileManager, mockFileSystem);
        mockFileManager.GetXmlFilesList().Returns(new List<FilesXmlFile>());
        systemUnderTest.CreateResourceFactory();


        // Assert
        Assert.Multiple(() =>
        {
            systemUnderTest.FileManager.Received().SetXmlFilesList(systemUnderTest.FilesXmlFilesList);
            systemUnderTest.FileManager.Received()
                .CalculateHashCheckSumAndFileSize(fileString + "." + resourceList.Last().ElementFileType);
            systemUnderTest.FileManager.Received()
                .CreateFolderAndFiles(fileString, systemUnderTest.FileManager.GetHashCheckSum());
            systemUnderTest.FileManager.Received().GetFileSize();
            systemUnderTest.FileManager.Received().GetHashCheckSum();
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void FileSetParametersFilesXml_SetFileXmlEntity_AndAddToFilesList()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        mockFileSystem.Directory.GetCurrentDirectory();

        // Act
        var systemUnderTest = new XmlResourceFactory(mockReadAtf, mockFileManager, mockFileSystem)
            {
                FilesXmlFilesList = new List<FilesXmlFile>(),
                FileElementId = "1",
                FileElementName = "FileName",
                FileElementType = "pdf"
            };
        systemUnderTest.ResourceSetParametersFilesXml("1234", "456789", "something", "2404");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FilesXmlFilesList[0].ContentHash, Is.EqualTo("1234"));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].ContextId, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Filename, Is.EqualTo($"{systemUnderTest.FileElementName}.{systemUnderTest.FileElementType}"));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Filesize, Is.EqualTo("456789"));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Source, Is.EqualTo($"{systemUnderTest.FileElementName}.{systemUnderTest.FileElementType}"));
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Timecreated, Is.Not.Empty);
            Assert.That(systemUnderTest.FilesXmlFilesList[0].Timemodified, Is.Not.Empty);
            Assert.That(systemUnderTest.FilesXmlFilesList[1].ContentHash, Is.EqualTo("1234"));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].ContextId, Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Filename, Is.EqualTo($"{systemUnderTest.FileElementName}.{systemUnderTest.FileElementType}"));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Filesize, Is.EqualTo("456789"));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Source, Is.EqualTo($"{systemUnderTest.FileElementName}.{systemUnderTest.FileElementType}"));
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Timecreated, Is.Not.Empty);
            Assert.That(systemUnderTest.FilesXmlFilesList[1].Timemodified, Is.Not.Empty);
            Assert.That(systemUnderTest.FilesXmlFilesList[0].ElementUuid, Is.EqualTo("2404"));
            Assert.That(systemUnderTest.FilesXmlFilesList, Has.Count.EqualTo(2));
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void
        FileSetParametersActivity_CreateActivityFolder_SetsGradesResourceRolesModuleGradeHistoryInforef_AndSerializes()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        mockFileSystem.Directory.GetCurrentDirectory();

        var mockGradesGradeItem = new ActivitiesGradesXmlGradeItem();
        var mockGradesGradeItems = new ActivitiesGradesXmlGradeItems();
        var mockGradesGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        var mockFileResource = new ActivitiesResourceXmlResource();
        var mockFileResourceActivity = Substitute.For<IActivitiesResourceXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        mockModule.PluginLocalAdlerModule = Substitute.For<ActivitiesModuleXmlPluginLocalAdlerModule>();
        var mockGradehistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();
        var mockInforefFile = new ActivitiesInforefXmlFile();
        var mockInforefFileref = new ActivitiesInforefXmlFileref();
        var mockInforefGradeItem = new ActivitiesInforefXmlGradeItem();
        var mockInforefGradeItemref = new ActivitiesInforefXmlGradeItemref();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();

        // Act
        var systemUnderTest = new XmlResourceFactory(mockReadAtf, mockFileManager, mockFileSystem, mockGradesGradeItem,
            mockGradesGradeItems, mockGradesGradebook, mockFileResource, mockFileResourceActivity, mockRoles,
            mockModule, mockGradehistory, mockInforefFile, mockInforefFileref, mockInforefGradeItem,
            mockInforefGradeItemref,
            mockInforefInforef);

        systemUnderTest.FileElementDesc = "DESC";
        systemUnderTest.FileSetParametersActivity();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradesGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradesGradeItems));
            systemUnderTest.ActivitiesGradesXmlActivityGradebook.Received()
                .Serialize("resource", systemUnderTest.FileElementId);

            Assert.That(systemUnderTest.ActivitiesFileResourceXmlResource, Is.EqualTo(mockFileResource));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.Id,
                Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ModuleId,
                Is.EqualTo(systemUnderTest.FileElementId));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ModuleName, Is.EqualTo("resource"));
            Assert.That(systemUnderTest.ActivitiesFileResourceXmlActivity.ContextId,
                Is.EqualTo(systemUnderTest.FileElementId));
            systemUnderTest.ActivitiesFileResourceXmlActivity.Received()
                .Serialize("resource", systemUnderTest.FileElementId);

            systemUnderTest.ActivitiesRolesXmlRoles.Received().Serialize("resource", systemUnderTest.FileElementId);

            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ModuleName, Is.EqualTo("resource"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionId, Is.EqualTo(string.Empty));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionNumber, Is.EqualTo(string.Empty));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Added, Is.Not.Empty);
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Id, Is.EqualTo(systemUnderTest.FileElementId));
            systemUnderTest.ActivitiesModuleXmlModule.Received().Serialize("resource", systemUnderTest.FileElementId);

            systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory.Received()
                .Serialize("resource", systemUnderTest.FileElementId);

            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref.File, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref.GradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef.Fileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef.GradeItemref, Is.EqualTo(mockInforefGradeItemref));
            systemUnderTest.ActivitiesInforefXmlInforef.Received().Serialize("resource", systemUnderTest.FileElementId);
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void CreateActivityFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlResourceFactory(mockReadAtf, fileSystem: mockFileSystem);

        //Act
        systemUnderTest.CreateActivityFolder("1");

        //Assert
        Assert.That(
            mockFileSystem.Directory.Exists(Path.Join(ApplicationPaths.BackupFolder, "XMLFilesForExport", "activities",
                "resource_" + "1")),
            Is.True);
    }
}