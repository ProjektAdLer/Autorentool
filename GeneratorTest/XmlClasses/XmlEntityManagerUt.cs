﻿using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
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
    // ANF-ID: [GHO11]
    public void XmlEntityManager_GetFactories_AllFactoriesInitialisedAndCreateMethodCalled()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileFactory = Substitute.For<IXmlResourceFactory>();
        var mockH5PFactory = Substitute.For<IXmlH5PFactory>();
        var mockCourseFactory = Substitute.For<IXmlCourseFactory>();
        var mockBackupFactory = Substitute.For<IXmlBackupFactory>();
        var mockSectionFactory = Substitute.For<IXmlSectionFactory>();
        var mockLabelFactory = Substitute.For<IXmlLabelFactory>();
        var mockUrlFactory = Substitute.For<IXmlUrlFactory>();
        var mockAdaptivityFactory = Substitute.For<IXmlAdaptivityFactory>();

        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "course"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_1"));
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "label_2"));

        mockReadAtf.GetH5PElementsList().Returns(new List<ILearningElementJson>());

        var topicsJson = new TopicJson(1, "Topic", new List<int> { 1 });
        var topicsList = new List<ITopicJson> { topicsJson };
        var learningSpacesJson1 = new LearningSpaceJson(1, "", "space1",
            new List<int?> { 1, 2 }, 0, "", "");
        var learningSpacesJson2 = new LearningSpaceJson(1, "", "space2",
            new List<int?> { 3, 4 }, 0, "", "");
        var learningSpacesList = new List<ILearningSpaceJson> { learningSpacesJson1, learningSpacesJson2 };
        var learningElementJson1 = new LearningElementJson(1,
            "", "", "", "", "h5p", 0, 2, "");
        var learningElementJson2 = new LearningElementJson(2,
            "", "", "", "", "json", 0, 3, "");
        var learningElementList = new List<IElementJson> { learningElementJson1, learningElementJson2 };
        var learningWorldJson = new LearningWorldJson("world", "", topicsList, learningSpacesList, learningElementList);

        mockReadAtf.GetLearningWorld().Returns(learningWorldJson);
        mockReadAtf.GetSpaceList().Returns(learningSpacesList);
        mockReadAtf.GetElementsOrderedList().Returns(learningElementList);

        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        var systemUnderTest = new XmlEntityManager();
        systemUnderTest.GetFactories(mockReadAtf, mockFileFactory, mockH5PFactory, mockCourseFactory,
            mockBackupFactory, mockSectionFactory, mockLabelFactory, mockUrlFactory, mockAdaptivityFactory);

        // Assert
        Assert.Multiple(() =>
        {
            mockFileFactory.Received().CreateResourceFactory();
            mockH5PFactory.Received().CreateH5PFileFactory();
            mockCourseFactory.Received().CreateXmlCourseFactory();
            mockBackupFactory.Received().CreateXmlBackupFactory();
            mockSectionFactory.Received().CreateSectionFactory();
            mockLabelFactory.Received().CreateLabelFactory();
            mockUrlFactory.Received().CreateUrlFactory();
            mockAdaptivityFactory.Received().CreateXmlAdaptivityFactory();
        });
    }
}