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
        
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "course"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_1"));
        
        mockReadDsl.GetH5PElementsList().Returns(new List<LearningElementJson>());
        
        var identifierLearningWorldJson = new IdentifierJson("name", "World");
        var identifierLearningSpaceJson_1 = new IdentifierJson("name", "Space_1");
        var identifierLearningSpaceJson_2 = new IdentifierJson("name", "Space_2");
        var identifierLearningElementJson_1 = new IdentifierJson("name", "Element_1");
        var identifierLearningElementJson_2 = new IdentifierJson("name", "DSL Dokument");
        var learningElementValueJson_1 = new LearningElementValueJson("text", "Hello World");
        var learningElementValueJson_2 = new LearningElementValueJson("text", "Hello Space");
        var learningElementValueList_1 = new List<LearningElementValueJson>(){learningElementValueJson_1};
        var learningElementValueList_2 = new List<LearningElementValueJson>(){learningElementValueJson_2};
        var learningWorldContentJson = new List<int>(){1,2};
        var topicsJson = new TopicJson();
        var topicsList = new List<TopicJson>(){topicsJson};
        var learningSpacesJson_1 = new LearningSpaceJson(1, "Space_1",
            identifierLearningSpaceJson_1, new List<int>() {1, 2});
        var learningSpacesJson_2 = new LearningSpaceJson(1, "Space_1", 
            identifierLearningSpaceJson_2, new List<int>() {3, 4});
        var learningSpacesList = new List<LearningSpaceJson>(){learningSpacesJson_1, learningSpacesJson_2};
        var learningElementJson_1 = new LearningElementJson(1,
            identifierLearningElementJson_1, "h5p", 0);
        learningElementJson_1.LearningElementValue = learningElementValueList_1;
        var learningElementJson_2 = new LearningElementJson(2,
            identifierLearningElementJson_2, "json", 0);
        learningElementJson_2.LearningElementValue = learningElementValueList_2;
        var learningElementList = new List<LearningElementJson>(){learningElementJson_1, learningElementJson_2};
        var learningWorldJson = new LearningWorldJson("uuid", identifierLearningWorldJson, learningWorldContentJson, topicsList, learningSpacesList, learningElementList);
        
        mockReadDsl.GetLearningWorld().Returns(learningWorldJson);
        mockReadDsl.GetLearningSpaceList().Returns(learningSpacesList);
        mockReadDsl.GetSpacesAndElementsOrderedList().Returns(learningElementList);
        
        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        var systemUnderTest = new XmlEntityManager();
        systemUnderTest.GetFactories(mockReadDsl, mockFileFactory, mockH5PFactory, mockCourseFactory, mockBackupFactory);

        // Assert
        Assert.Multiple(() =>
        {
            mockFileFactory.Received().CreateFileFactory();
            mockH5PFactory.Received().CreateH5PFileFactory();
            mockCourseFactory.Received().CreateXmlCourseFactory();
            mockBackupFactory.Received().CreateXmlBackupFactory();
        });
        
    }
}