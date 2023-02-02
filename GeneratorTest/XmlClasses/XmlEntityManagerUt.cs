using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Generator.WorldExport;
using Generator.XmlClasses;
using Generator.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses;


[TestFixture]
public class XmlEntityManagerUt
{
    [Test]
    public void XmlEntityManager_GetFactories_AllFactoriesInitialisedAndCreateMethodCalled()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileFactory = Substitute.For<IXmlResourceFactory>();
        var mockH5PFactory = Substitute.For<IXmlH5PFactory>();
        var mockCourseFactory = Substitute.For<IXmlCourseFactory>();
        var mockBackupFactory = Substitute.For<IXmlBackupFactory>();
        var mockSectionFactory = Substitute.For<IXmlSectionFactory>();
        var mockLabelFactory = Substitute.For<IXmlLabelFactory>();
        var mockUrlFactory = Substitute.For<IXmlUrlFactory>();
        
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "course"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_1"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "label_2"));
        
        mockReadDsl.GetH5PElementsList().Returns(new List<ElementJson>());
        
        var identifierWorldJson = new IdentifierJson("name", "World");
        var identifierSpaceJson1 = new IdentifierJson("name", "Space_1");
        var identifierSpaceJson2 = new IdentifierJson("name", "Space_2");
        var identifierElementJson1 = new IdentifierJson("name", "Element_1");
        var identifierElementJson2 = new IdentifierJson("name", "DSL Dokument");
        var elementValueJson1 = new ElementValueJson("points", "10");
        var elementValueJson2 = new ElementValueJson("points", "10");
        var elementValueList1 = new List<ElementValueJson>(){elementValueJson1};
        var elementValueList2 = new List<ElementValueJson>(){elementValueJson2};
        var worldContentJson = new List<int>(){1,2};
        var topicsJson = new TopicJson();
        var topicsList = new List<TopicJson>(){topicsJson};
        var spacesJson1 = new SpaceJson(1, identifierSpaceJson1, 
            new List<int>() {1, 2}, 0, 0);
        var spacesJson2 = new SpaceJson(1, identifierSpaceJson2, 
            new List<int>() {3, 4}, 0, 0);
        var spacesList = new List<SpaceJson>(){spacesJson1, spacesJson2};
        var elementJson1 = new ElementJson(1,
            identifierElementJson1, "", "", "h5p", 0, elementValueList1);
        var elementJson2 = new ElementJson(2,
            identifierElementJson2, "", "", "json", 0, elementValueList2);
        var elementList = new List<ElementJson>(){elementJson1, elementJson2};
        var worldJson = new WorldJson("uuid", identifierWorldJson, worldContentJson, topicsList, spacesList, elementList);
        
        var mockElementValueList = new List<ElementValueJson>{new ("type","10")};
        var mockLabelsElementJson = new ElementJson(2, new IdentifierJson("Name", "Labels_1"), "", "", "mp4", 1, mockElementValueList);
        
        var labelJsonList = new List<ElementJson> {mockLabelsElementJson};
        
        mockReadDsl.GetLabelsList().Returns(labelJsonList);
        mockReadDsl.GetWorld().Returns(worldJson);
        mockReadDsl.GetSectionList().Returns(spacesList);
        mockReadDsl.GetSpacesAndElementsOrderedList().Returns(elementList);
        mockReadDsl.GetLabelsList().Returns(labelJsonList);
        
        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        var systemUnderTest = new XmlEntityManager();
        systemUnderTest.GetFactories(mockReadDsl, mockFileFactory, mockH5PFactory, mockCourseFactory, 
            mockBackupFactory, mockSectionFactory, mockLabelFactory,mockUrlFactory);

        // Assert
        Assert.Multiple(() =>
        {
            mockFileFactory.Received().CreateResourceFactory();
            mockH5PFactory.Received().CreateH5PFileFactory();
            mockCourseFactory.Received().CreateXmlCourseFactory();
            mockBackupFactory.Received().CreateXmlBackupFactory();
        });
        
    }
}