using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
using Generator.WorldExport;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.H5PActivity.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities.Files.xml;
using Generator.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.XmlFileFactories;

[TestFixture]
public class XmlH5PFactoryUt
{
    [Test]
    public void XmlH5PFactory_Constructor_AllPropertiesSet()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileManager = new XmlFileManager();
        var mockFileSystem = new MockFileSystem();

        //Act
        var systemUnderTest = new XmlH5PFactory(mockReadAtf, mockFileManager, mockFileSystem);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FilesXmlFileBlock1, Is.Not.Null);
            Assert.That(systemUnderTest.FilesXmlFileBlock2, Is.Not.Null);
            Assert.That(systemUnderTest.FilesXmlFiles, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlActivityGradebook, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity, Is.Not.Null);
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
    public void XmlH5PFactory_CreateH5PFileFactory_AddFilesToFileList()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();

        var mockFileSystem = new MockFileSystem();
        var mockFile = new FilesXmlFile();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var mockFiles = Substitute.For<IFilesXmlFiles>();
        var currWorkDir = ApplicationPaths.BackupFolder;

        var h5PElement1 = new LearningElementJson(1, "", "element1", "", "h5p", "h5p", 1, 2, "");

        var h5PElement2 = new LearningElementJson(2, "", "element2", "", "h5p", "h5p", 1, 3, "");

        var h5PList = new List<ILearningElementJson>
        {
            h5PElement1, h5PElement2
        };

        mockReadAtf.GetH5PElementsList().Returns(h5PList);
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport", "element1"),
            new MockFileData("Hello World"));

        var systemUnderTest = new XmlH5PFactory(mockReadAtf, xmlFileManager: mockFileManager,
            fileSystem: mockFileSystem, filesXmlFile: mockFile, filesXmlFiles: mockFiles);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        mockFileManager.GetXmlFilesList().Returns(new List<FilesXmlFile>());
        var space_1 = new LearningSpaceJson(1, "", "space1", new List<int?> { 1, 2 }, 10, "space", "");
        mockReadAtf.GetH5PElementsList().Returns(h5PList);
        systemUnderTest.CreateH5PFileFactory();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FilesXmlFiles, Is.EqualTo(mockFiles));
            systemUnderTest.FileManager.Received().GetXmlFilesList();
            systemUnderTest.FileManager.Received().CalculateHashCheckSumAndFileSize(Path.Join(currWorkDir,
                "XMLFilesForExport", "element2" + "." + h5PElement2.ElementFileType));
            systemUnderTest.FileManager.Received().GetHashCheckSum();
            systemUnderTest.FileManager.Received().GetFileSize();
            systemUnderTest.FileManager.Received().CreateFolderAndFiles(Arg.Any<string>(), Arg.Any<string>());
            systemUnderTest.FilesXmlFiles.Received().Serialize();
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void H5PSetParametersFilesXml_SetsFile2Times_AndAddsToFileList()
    {
        var mockReadAtf = Substitute.For<IReadAtf>();

        var mockFileSystem = new MockFileSystem();
        var mockFile = new FilesXmlFile();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var mockFiles = Substitute.For<IFilesXmlFiles>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        var h5PElement1 = new LearningElementJson(1, "", "element1", "", "h5p", "h5p", 1, 1, "");

        var h5PElement2 = new LearningElementJson(2, "", "element2", "", "h5p", "h5p", 1, 1, "");

        var h5PList = new List<ILearningElementJson>
        {
            h5PElement1, h5PElement2
        };

        var space_1 =
            new LearningSpaceJson(1, "", "space", new List<int?> { 1, 2 }, 10, "space", "");
        mockReadAtf.GetH5PElementsList().Returns(h5PList);
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport", "space"), new MockFileData("Hello World"));

        var systemUnderTest = new XmlH5PFactory(mockReadAtf, xmlFileManager: mockFileManager,
            fileSystem: mockFileSystem, filesXmlFile: mockFile, filesXmlFiles: mockFiles);
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;


        //Act
        systemUnderTest.FileManager.GetXmlFilesList().Returns(new List<FilesXmlFile>());
        systemUnderTest.CreateH5PFileFactory();

        //Assert
        Assert.Multiple(() =>
        {
            //Every File has 2 FilesXmlFile Id´s thats why the Count has to be 2*FileCount
            Assert.That(systemUnderTest.FilesXmlFiles.File, Has.Count.EqualTo(4));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].ContextId, Is.EqualTo(h5PElement1.ElementId.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].Filename, Is.EqualTo("element1"));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].Source,
                Is.EqualTo("element1" + "." + h5PElement1.ElementFileType));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].ContextId, Is.EqualTo(h5PElement2.ElementId.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].Filename, Is.EqualTo("element2"));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].Source,
                Is.EqualTo("element2" + "." + h5PElement2.ElementFileType));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].Id,
                Is.EqualTo((int.Parse(systemUnderTest.FilesXmlFiles.File[0].Id) + 2).ToString()));
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void H5PSetParametersActivity_SetsGradesH5pActivityRolesModuleGradehistoryInforef_AndSerializes()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();

        var mockGradeItem = Substitute.For<IActivitiesGradesXmlGradeItem>();
        var mockGradeItems = Substitute.For<IActivitiesGradesXmlGradeItems>();
        var mockActivityGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();

        var mockActivityH5PActivity = Substitute.For<IActivitiesH5PActivityXmlH5PActivity>();
        var mockH5PActivity = Substitute.For<IActivitiesH5PActivityXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        mockModule.PluginLocalAdlerModule = Substitute.For<ActivitiesModuleXmlPluginLocalAdlerModule>();
        var mockGradeHistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();

        var mockInforefFile = Substitute.For<IActivitiesInforefXmlFile>();
        var mockInforefFileref = Substitute.For<IActivitiesInforefXmlFileref>();
        var mockInforefGradeItem = Substitute.For<IActivitiesInforefXmlGradeItem>();
        var mockInforefGradeItemref = Substitute.For<IActivitiesInforefXmlGradeItemref>();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();


        //Act
        var systemUnderTest = new XmlH5PFactory(mockReadAtf, fileSystem: mockFileSystem, gradesGradeItem: mockGradeItem,
            gradesGradeItems: mockGradeItems, gradebook: mockActivityGradebook, h5PActivityXmlActivity: mockH5PActivity,
            h5PActivityXmlH5PActivity: mockActivityH5PActivity, roles: mockRoles, module: mockModule,
            gradeHistory: mockGradeHistory,
            inforefXmlFile: mockInforefFile, inforefXmlFileref: mockInforefFileref,
            inforefXmlGradeItem: mockInforefGradeItem,
            inforefXmlGradeItemref: mockInforefGradeItemref, inforefXmlInforef: mockInforefInforef);

        systemUnderTest.H5PElementId = "100";
        systemUnderTest.H5PElementName = "h5pName";
        systemUnderTest.CurrentTime = "1284511";
        systemUnderTest.H5PElementPoints = 10;

        systemUnderTest.H5PSetParametersActivity();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.CategoryId,
                Is.EqualTo(systemUnderTest.H5PElementId));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.ItemName,
                Is.EqualTo(systemUnderTest.H5PElementName));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.ItemType, Is.EqualTo("mod"));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.ItemModule, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.Timecreated,
                Is.Not.Empty);
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.Timemodified,
                Is.Not.Empty);
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.Id, Is.EqualTo(systemUnderTest.H5PElementId));

            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradeItems));
            systemUnderTest.ActivitiesGradesXmlActivityGradebook.Received()
                .Serialize("h5pactivity", systemUnderTest.H5PElementId);

            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Name,
                Is.EqualTo(systemUnderTest.H5PElementName));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Timecreated,
                Is.Not.Empty);
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Timemodified,
                Is.Not.Empty);
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Id,
                Is.EqualTo(systemUnderTest.H5PElementId));

            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.Id, Is.EqualTo(systemUnderTest.H5PElementId));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.ModuleId,
                Is.EqualTo(systemUnderTest.H5PElementId));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.ModuleName, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.ContextId,
                Is.EqualTo(systemUnderTest.H5PElementId));
            systemUnderTest.ActivitiesH5PActivityXmlActivity.Received()
                .Serialize("h5pactivity", systemUnderTest.H5PElementId);

            systemUnderTest.ActivitiesRolesXmlRoles.Received().Serialize("h5pactivity", systemUnderTest.H5PElementId);

            Assert.That(systemUnderTest.ActivitiesModuleXmlModule, Is.EqualTo(mockModule));
            systemUnderTest.ActivitiesModuleXmlModule.Received().Serialize("h5pactivity", systemUnderTest.H5PElementId);

            systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory.Received()
                .Serialize("h5pactivity", systemUnderTest.H5PElementId);

            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref, Is.EqualTo(mockInforefGradeItemref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.EqualTo(mockInforefInforef));
            systemUnderTest.ActivitiesInforefXmlInforef.Received()
                .Serialize("h5pactivity", systemUnderTest.H5PElementId);
        });
    }

    /*
    [Test]
    public void H5PSetParametersSections_SetsSectionInforefSection_AndSerializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();

        var mockSectionInforef = Substitute.For<ISectionsInforefXmlInforef>();
        var mockSection = Substitute.For<ISectionsSectionXmlSection>();

        //Act
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, fileSystem: mockFileSystem,
            sectionsInforefXmlInforef: mockSectionInforef,
            sectionsSectionXmlSection: mockSection);

        systemUnderTest.H5PElementId = "100";
        systemUnderTest.H5PElementName = "h5pName";
        systemUnderTest.CurrentTime = "1284511";

        systemUnderTest.H5PSetParametersSections();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SectionsInforefXmlInforef, Is.EqualTo(mockSectionInforef));
            Assert.That(systemUnderTest.SectionsSectionXmlSection, Is.EqualTo(mockSection));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Number, Is.EqualTo(systemUnderTest.H5PElementId));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Name, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Summary, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Timemodified, Is.EqualTo(systemUnderTest.CurrentTime));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Id, Is.EqualTo(systemUnderTest.H5PElementId));

            systemUnderTest.SectionsInforefXmlInforef.Received().Serialize("", systemUnderTest.H5PElementId);
            systemUnderTest.SectionsSectionXmlSection.Received().Serialize("", systemUnderTest.H5PElementId);
        });

    }*/

    [Test]
    // ANF-ID: [GHO11]
    public void CreateActivityFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();

        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlH5PFactory(mockReadAtf, fileSystem: mockFileSystem);

        //Act
        systemUnderTest.CreateActivityFolder("1");

        //Assert
        Assert.That(
            mockFileSystem.Directory.Exists(Path.Join(ApplicationPaths.BackupFolder, "XMLFilesForExport", "activities",
                "h5pactivity_" + "1")),
            Is.True);
    }

    /*
    [Test]
    public void CreateSectionFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();

        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = new XmlH5PFactory(mockReadDsl, fileSystem: mockFileSystem);

        //Act
        systemUnderTest.CreateSectionsFolder("1");

        //Assert
        Assert.That(mockFileSystem.Directory.Exists(Path.Join("XMLFilesForExport", "sections", "section_"+"1")), Is.True);
    }*/
}