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
        
        mockReadDsl.GetH5PElementsList().Returns(new List<LearningElementJson>());
        
        var identifierLearningWorldJson = new IdentifierJson("name", "World");
        var identifierLearningSpaceJson1 = new IdentifierJson("name", "Space_1");
        var identifierLearningSpaceJson2 = new IdentifierJson("name", "Space_2");
        var identifierLearningElementJson1 = new IdentifierJson("name", "Element_1");
        var identifierLearningElementJson2 = new IdentifierJson("name", "DSL Dokument");
        var learningElementValueJson1 = new LearningElementValueJson("points", 10);
        var learningElementValueJson2 = new LearningElementValueJson("points", 10);
        var learningElementValueList1 = new List<LearningElementValueJson>(){learningElementValueJson1};
        var learningElementValueList2 = new List<LearningElementValueJson>(){learningElementValueJson2};
        var learningWorldContentJson = new List<int>(){1,2};
        var topicsJson = new TopicJson();
        var topicsList = new List<TopicJson>(){topicsJson};
        var learningSpacesJson1 = new LearningSpaceJson(1, identifierLearningSpaceJson1, 
            new List<int>() {1, 2}, 0, 0);
        var learningSpacesJson2 = new LearningSpaceJson(1, identifierLearningSpaceJson2, 
            new List<int>() {3, 4}, 0, 0);
        var learningSpacesList = new List<LearningSpaceJson>(){learningSpacesJson1, learningSpacesJson2};
        var learningElementJson1 = new LearningElementJson(1,
            identifierLearningElementJson1, "", "", "h5p", 0, learningElementValueList1);
        var learningElementJson2 = new LearningElementJson(2,
            identifierLearningElementJson2, "", "", "json", 0, learningElementValueList2);
        var learningElementList = new List<LearningElementJson>(){learningElementJson1, learningElementJson2};
        var learningWorldJson = new LearningWorldJson("uuid", identifierLearningWorldJson, learningWorldContentJson, topicsList, learningSpacesList, learningElementList);
        
        var mockElementValueList = new List<LearningElementValueJson>{new ("type",10)};
        var mockLabelsElementJson = new LearningElementJson(2, new IdentifierJson("Name", "Labels_1"), "", "", "mp4", 1, mockElementValueList);
        
        var labelJsonList = new List<LearningElementJson> {mockLabelsElementJson};
        
        mockReadDsl.GetLabelsList().Returns(labelJsonList);
        mockReadDsl.GetLearningWorld().Returns(learningWorldJson);
        mockReadDsl.GetSectionList().Returns(learningSpacesList);
        mockReadDsl.GetSpacesAndElementsOrderedList().Returns(learningElementList);
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