﻿using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities._activities.Url.xml;
using Generator.XmlClasses.XmlFileFactories;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.XmlFileFactories;

[TestFixture]
public class XmlUrlFactoryUt
{
    [Test]
    public void XmlUrlFactory_Constructor_AllParametersSet()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();

        // Act
        var systemUnderTest = new XmlUrlFactory(mockReadAtf, mockFileSystem);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ReadAtf, Is.EqualTo(mockReadAtf));
            Assert.That(systemUnderTest.UrlId, Is.EqualTo(""));
            Assert.That(systemUnderTest.UrlName, Is.EqualTo(""));
            Assert.That(systemUnderTest.UrlParentSpaceId, Is.EqualTo(""));
            Assert.That(systemUnderTest.UrlLink, Is.EqualTo(""));
            Assert.That(systemUnderTest.CurrentTime, Is.Not.Empty);
            Assert.That(systemUnderTest.UrlList, Is.Not.Null);

            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradesXmlActivityGradebook, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesUrlXmlUrl, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesUrlXmlActivity, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesRolesXmlRoles, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.Not.Null);
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.Not.Null);
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void XmlUrlFactory_CreateUrlFactory_UrlListCreated()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();
        var systemUnderTest = new XmlUrlFactory(mockReadAtf, mockFileSystem);

        var urlLearningElementJson = new LearningElementJson(1, "",
            "YoutubeVideo", "youtube.de", "video", "Video-Link", 1,
            2, "",
            "desc", new[] { "goals" });
        var urlList = new List<ILearningElementJson> { urlLearningElementJson };

        mockReadAtf.GetUrlElementList().Returns(urlList);
        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.CreateUrlFactory();

        // Assert
        Assert.That(systemUnderTest.UrlList, Is.EqualTo(urlList));
    }

    [Test]
    // ANF-ID: [GHO11]
    public void XmlUrlFactory_UrlSetParameters_AllXmlParametersSetAndSerialized()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();

        var mockGradesGradeItem = new ActivitiesGradesXmlGradeItem();
        var mockGradesGradeItems = new ActivitiesGradesXmlGradeItems();
        var mockGradesGradebook = Substitute.For<IActivitiesGradesXmlActivityGradebook>();
        var mockUrl = new ActivitiesUrlXmlUrl();
        var mockUrlActivity = Substitute.For<IActivitiesUrlXmlActivity>();
        var mockRoles = Substitute.For<IActivitiesRolesXmlRoles>();
        var mockModule = Substitute.For<IActivitiesModuleXmlModule>();
        mockModule.PluginLocalAdlerModule = Substitute.For<ActivitiesModuleXmlPluginLocalAdlerModule>();
        var mockGradehistory = Substitute.For<IActivitiesGradeHistoryXmlGradeHistory>();
        var mockInforefFileref = new ActivitiesInforefXmlFileref();
        var mockInforefGradeItem = new ActivitiesInforefXmlGradeItem();
        var mockInforefInforef = Substitute.For<IActivitiesInforefXmlInforef>();

        var systemUnderTest = new XmlUrlFactory(mockReadAtf, mockFileSystem, mockGradesGradeItem,
            mockGradesGradeItems, mockGradesGradebook, mockUrl, mockUrlActivity, mockRoles, mockModule,
            mockGradehistory, mockInforefFileref, mockInforefGradeItem, mockInforefInforef);

        var urlLearningElementJson = new LearningElementJson(1, "", "Video auf Youtube",
            "youtube.de", "video", "url", 1,
            2, "",
            "desc", new[] { "goals" });
        var urlList = new List<ILearningElementJson> { urlLearningElementJson };


        // Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.UrlSetParameters(urlList);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.UrlId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.UrlName, Is.EqualTo("Video auf Youtube"));
            Assert.That(systemUnderTest.UrlParentSpaceId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.UrlLink, Is.EqualTo("youtube.de"));

            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItem, Is.EqualTo(mockGradesGradeItem));
            Assert.That(systemUnderTest.ActivitiesGradesXmlGradeItems, Is.EqualTo(mockGradesGradeItems));
            Assert.That(systemUnderTest.ActivitiesGradesXmlActivityGradebook, Is.EqualTo(mockGradesGradebook));
            systemUnderTest.ActivitiesGradesXmlActivityGradebook.Received().Serialize("url", systemUnderTest.UrlId);

            Assert.That(systemUnderTest.ActivitiesUrlXmlUrl, Is.EqualTo(mockUrl));
            Assert.That(systemUnderTest.ActivitiesUrlXmlUrl.Name, Is.EqualTo(systemUnderTest.UrlName));
            Assert.That(systemUnderTest.ActivitiesUrlXmlUrl.Intro,
                Is.EqualTo(systemUnderTest.UrlLink + "<p style=\"position:relative; background-color:#e6e9ed;\">" +
                           systemUnderTest.UrlDescription + "</p>"));
            Assert.That(systemUnderTest.ActivitiesUrlXmlUrl.Externalurl, Is.EqualTo(systemUnderTest.UrlLink));
            Assert.That(systemUnderTest.ActivitiesUrlXmlUrl.Timemodified, Is.Not.Empty);

            Assert.That(systemUnderTest.ActivitiesUrlXmlActivity, Is.EqualTo(mockUrlActivity));
            Assert.That(systemUnderTest.ActivitiesUrlXmlActivity.Url, Is.EqualTo(mockUrl));
            Assert.That(systemUnderTest.ActivitiesUrlXmlActivity.Id, Is.EqualTo(systemUnderTest.UrlId));
            Assert.That(systemUnderTest.ActivitiesUrlXmlActivity.Moduleid, Is.EqualTo(systemUnderTest.UrlId));
            Assert.That(systemUnderTest.ActivitiesUrlXmlActivity.Contextid, Is.EqualTo(systemUnderTest.UrlId));
            systemUnderTest.ActivitiesUrlXmlActivity.Received().Serialize("url", systemUnderTest.UrlId);

            Assert.That(systemUnderTest.ActivitiesRolesXmlRoles, Is.EqualTo(mockRoles));
            systemUnderTest.ActivitiesRolesXmlRoles.Received().Serialize("url", systemUnderTest.UrlId);

            Assert.That(systemUnderTest.ActivitiesModuleXmlModule, Is.EqualTo(mockModule));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ModuleName, Is.EqualTo("url"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.ShowDescription, Is.EqualTo("1"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Indent, Is.EqualTo("1"));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionId,
                Is.EqualTo(systemUnderTest.UrlParentSpaceId));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.SectionNumber,
                Is.EqualTo(systemUnderTest.UrlParentSpaceId));
            Assert.That(systemUnderTest.ActivitiesModuleXmlModule.Completion, Is.EqualTo("2"));
            systemUnderTest.ActivitiesModuleXmlModule.Received().Serialize("url", systemUnderTest.UrlId);

            Assert.That(systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory, Is.EqualTo(mockGradehistory));
            systemUnderTest.ActivitiesGradeHistoryXmlGradeHistory.Received().Serialize("url", systemUnderTest.UrlId);

            Assert.That(systemUnderTest.ActivitiesInforefXmlFileref, Is.EqualTo(mockInforefFileref));
            Assert.That(systemUnderTest.ActivitiesInforefXmlGradeItem, Is.EqualTo(mockInforefGradeItem));
            Assert.That(systemUnderTest.ActivitiesInforefXmlInforef, Is.EqualTo(mockInforefInforef));
            systemUnderTest.ActivitiesInforefXmlInforef.Received().Serialize("url", systemUnderTest.UrlId);
        });
    }
}