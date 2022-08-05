using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AngleSharp.Text;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.GradeHistory.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Grades.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.H5PActivity.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Module.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Roles.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Section.xml;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class XmlH5PFactoryUt
{
    [Test]
    public void XmlH5PFactory_Constructor_AllPropertiesSet()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileManager = new XmlFileManager();
        var mockFileSystem = new MockFileSystem();

        //Act
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, mockFileManager, mockFileSystem);

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
            Assert.That(systemUnderTest.SectionsInforefXmlInforef, Is.Not.Null);
            Assert.That(systemUnderTest.SectionsSectionXmlSection, Is.Not.Null);
        });
    }
    
    [Test]
    public void XmlH5PFactory_CreateH5PFileFactory_AddFilesToFileList()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();

        var mockFileSystem = new MockFileSystem();
        var mockFile = new FilesXmlFile();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var mockFiles = Substitute.For<IFilesXmlFiles>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var identifier = new IdentifierJson();
        identifier.type = "FileName";    
        identifier.value = "Element_1";

        var h5pElement_1 = new LearningElementJson();
        h5pElement_1.id = 1;
        h5pElement_1.identifier = identifier;
        h5pElement_1.elementType = "h5p";
        
        var h5pElement_2 = new LearningElementJson();
        h5pElement_2.id = 2;
        h5pElement_2.identifier = identifier;
        h5pElement_2.elementType = "h5p";

        var h5pList = new List<LearningElementJson>()
        {
            h5pElement_1, h5pElement_2
        };
        
        mockReadDsl.GetH5PElementsList().Returns(h5pList);
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport", identifier.value), new MockFileData("Hello World"));
        
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, xmlFileManager: mockFileManager, fileSystem: mockFileSystem, filesXmlFile: mockFile, filesXmlFiles: mockFiles);
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.CreateH5PFileFactory();

        //Assert
        Assert.Multiple(()=>
        { 
            
            Assert.That(systemUnderTest.FilesXmlFiles, Is.EqualTo(mockFiles));
            systemUnderTest._fileManager.Received().GetXmlFilesList();
            systemUnderTest._fileManager.Received().CalculateHashCheckSumAndFileSize(Path.Join(currWorkDir, "XMLFilesForExport", identifier.value));
            systemUnderTest._fileManager.Received().GetHashCheckSum();
            systemUnderTest._fileManager.Received().GetFileSize();
            systemUnderTest._fileManager.Received().CreateFolderAndFiles(Arg.Any<string>(), Arg.Any<string>());
            systemUnderTest.FilesXmlFiles.Received().Serialize();
        });
    }
    
    [Test]
    public void H5PSetParametersFilesXml_SetsFile2Times_AndAddsToFileList()
    {
        var mockReadDsl = Substitute.For<IReadDSL>();

        var mockFileSystem = new MockFileSystem();
        var mockFile = new FilesXmlFile();
        var mockFileManager = Substitute.For<IXmlFileManager>();
        var mockFiles = Substitute.For<IFilesXmlFiles>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var identifier_1 = new IdentifierJson();
        identifier_1.type = "FileName";    
        identifier_1.value = "Element_1";
        
        var identifier_2 = new IdentifierJson();
        identifier_2.type = "FileName";    
        identifier_2.value = "Element_2";

        var h5pElement_1 = new LearningElementJson();
        h5pElement_1.id = 1;
        h5pElement_1.identifier = identifier_1;
        h5pElement_1.elementType = "h5p";
        
        var h5pElement_2 = new LearningElementJson();
        h5pElement_2.id = 2;
        h5pElement_2.identifier = identifier_2;
        h5pElement_2.elementType = "h5p";

        var h5pList = new List<LearningElementJson>()
        {
            h5pElement_1, h5pElement_2
        };
        
        mockReadDsl.GetH5PElementsList().Returns(h5pList);
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport", identifier_1.value), new MockFileData("Hello World"));
        
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, xmlFileManager: mockFileManager, fileSystem: mockFileSystem, filesXmlFile: mockFile, filesXmlFiles: mockFiles);
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        
        
        //Act
        systemUnderTest._fileManager.GetXmlFilesList().Returns(new List<FilesXmlFile>());
        systemUnderTest.CreateH5PFileFactory();
        
        //Assert
        Assert.Multiple(() =>
        {
            //Every File has 2 FilesXmlFile Id´s thats why the Count has to be 2*FileCount
            Assert.That(systemUnderTest.FilesXmlFiles.File, Has.Count.EqualTo(4));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].Id , Is.EqualTo("1"));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].ContextId , Is.EqualTo(h5pElement_1.id.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].Filename , Is.EqualTo(identifier_1.value.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].Source , Is.EqualTo(identifier_1.value.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].Id , Is.EqualTo("3"));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].ContextId , Is.EqualTo(h5pElement_2.id.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].Filename , Is.EqualTo(identifier_2.value.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[2].Source , Is.EqualTo(identifier_2.value.ToString()));
            Assert.That(systemUnderTest.FilesXmlFiles.File[0].Id , Is.EqualTo(systemUnderTest.FilesXmlFiles.File[0].Id.ToInteger(+2).ToString()));
        });
    }

    [Test]
    public void H5PSetParametersActivity_SetsGradesH5pActivityRolesModuleGradehistoryInforef_AndSerializes()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        
        var mockGradeItem = Substitute.For<IActivitiesGradesXmlGradeItem>();
        var mockGradeItems = Substitute.For<IActivitiesGradesXmlGradeItems>();
        var mockActivityGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        
        var mockActivityH5PActivity = Substitute.For<IActivitiesH5PActivityXmlH5PActivity>();
        var mockH5PActivity = Substitute.For<IActivitiesH5PActivityXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        var mockGradeHistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();

        var mockInforefFile = Substitute.For<IActivitiesInforefXmlFile>();
        var mockInforefFileref = Substitute.For<IActivitiesInforefXmlFileref>();
        var mockInforefGradeItem = Substitute.For<IActivitiesInforefXmlGradeItem>();
        var mockInforefGradeItemref = Substitute.For<IActivitiesInforefXmlGradeItemref>();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();


        //Act
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, fileSystem: mockFileSystem, gradesGradeItem: mockGradeItem,
            gradesGradeItems: mockGradeItems, gradebook: mockActivityGradebook, h5PActivityXmlActivity: mockH5PActivity,
            h5PActivityXmlH5PActivity: mockActivityH5PActivity, roles: mockRoles, module: mockModule, gradeHistory: mockGradeHistory,
            inforefXmlFile: mockInforefFile, inforefXmlFileref: mockInforefFileref, inforefXmlGradeItem: mockInforefGradeItem,
            inforefXmlGradeItemref: mockInforefGradeItemref, inforefXmlInforef: mockInforefInforef);
            
        systemUnderTest.h5pElementId = "100";
        systemUnderTest.h5pElementName = "h5pName";
        systemUnderTest.currentTime = "1284511";
        
        systemUnderTest.H5PSetParametersActivity();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.CategoryId, Is.EqualTo(systemUnderTest.h5pElementId));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.ItemName, Is.EqualTo(systemUnderTest.h5pElementName));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.ItemType, Is.EqualTo("mod"));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.ItemModule, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.Timecreated, Is.EqualTo(systemUnderTest.currentTime));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.Timemodified, Is.EqualTo(systemUnderTest.currentTime));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem.Id, Is.EqualTo(systemUnderTest.h5pElementId));
            
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradeItems));
            systemUnderTest.ActivitiesGradesXmlActivityGradebook.Received().Serialize("h5pactivity", systemUnderTest.h5pElementId);
            
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Name, Is.EqualTo(systemUnderTest.h5pElementName));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Timecreated, Is.EqualTo(systemUnderTest.currentTime));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Timemodified, Is.EqualTo(systemUnderTest.currentTime));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlH5PActivity.Id, Is.EqualTo(systemUnderTest.h5pElementId));
            
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.Id, Is.EqualTo(systemUnderTest.h5pElementId));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.ModuleId, Is.EqualTo(systemUnderTest.h5pElementId));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.ModuleName, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.ActivitiesH5PActivityXmlActivity.ContextId, Is.EqualTo(systemUnderTest.h5pElementId));
            systemUnderTest.ActivitiesH5PActivityXmlActivity.Received().Serialize("h5pactivity", systemUnderTest.h5pElementId);
            
            systemUnderTest.ActivitiesRolesXmlRoles.Received().Serialize("h5pactivity", systemUnderTest.h5pElementId);
            
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ModuleName, Is.EqualTo("h5pactivity"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionId, Is.EqualTo(systemUnderTest.h5pElementId));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionNumber, Is.EqualTo(systemUnderTest.h5pElementId));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.IdNumber, Is.EqualTo(""));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Added, Is.EqualTo(systemUnderTest.currentTime));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ShowDescription, Is.EqualTo("0"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Id, Is.EqualTo(systemUnderTest.h5pElementId));
            systemUnderTest.ActivitiesModuleXmlModule.Received().Serialize("h5pactivity", systemUnderTest.h5pElementId);
            
            systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory.Received().Serialize("h5pactivity", systemUnderTest.h5pElementId);
            
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItemref, Is.EqualTo(mockInforefGradeItemref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.EqualTo(mockInforefInforef));
            systemUnderTest.ActivitiesInforefXmlInforef.Received().Serialize("h5pactivity", systemUnderTest.h5pElementId);
            
        });
    }

    [Test]
    public void H5PSetParametersSections_SetsSectionInforefSection_AndSerializes()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        
        var mockSectionInforef = Substitute.For<ISectionsInforefXmlInforef>();
        var mockSection = Substitute.For<ISectionsSectionXmlSection>();
        
        //Act
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, fileSystem: mockFileSystem,
            sectionsInforefXmlInforef: mockSectionInforef,
            sectionsSectionXmlSection: mockSection);
        
        systemUnderTest.h5pElementId = "100";
        systemUnderTest.h5pElementName = "h5pName";
        systemUnderTest.currentTime = "1284511";
        
        systemUnderTest.H5PSetParametersSections();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SectionsInforefXmlInforef, Is.EqualTo(mockSectionInforef));
            Assert.That(systemUnderTest.SectionsSectionXmlSection, Is.EqualTo(mockSection));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Number, Is.EqualTo(systemUnderTest.h5pElementId));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Name, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Summary, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Timemodified, Is.EqualTo(systemUnderTest.currentTime));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Id, Is.EqualTo(systemUnderTest.h5pElementId));
            
            systemUnderTest.SectionsInforefXmlInforef.Received().Serialize("", systemUnderTest.h5pElementId);
            systemUnderTest.SectionsSectionXmlSection.Received().Serialize("", systemUnderTest.h5pElementId);
        });

    }

    [Test]
    public void CreateActivityFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();

        var mockFileSystem = new MockFileSystem();
        
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, fileSystem: mockFileSystem);
        
        //Act
        systemUnderTest.CreateActivityFolder("1");
        
        //Assert
        Assert.That(mockFileSystem.Directory.Exists(Path.Join("XMLFilesForExport", "activities", "h5pactivity_"+"1")), Is.True);
    }
    
    
    [Test]
    public void CreateSectionFolder_ActivityFolderCreated()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();

        var mockFileSystem = new MockFileSystem();
        
        var systemUnderTest = new XmlH5PFactory(mockReadDsl, fileSystem: mockFileSystem);
        
        //Act
        systemUnderTest.CreateSectionsFolder("1");
        
        //Assert
        Assert.That(mockFileSystem.Directory.Exists(Path.Join("XMLFilesForExport", "sections", "section_"+"1")), Is.True);
    }
}