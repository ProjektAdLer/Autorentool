using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;
using NSubstitute;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses;

[TestFixture]
public class XmlSectionFactoryUt
{
    [Test]
    public void XmlSectionFactory_StandardConstructor_AllPropertiesAreSet()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockSection = Substitute.For<ISectionsSectionXmlSection>();
        var mockInforef = Substitute.For<ISectionsInforefXmlInforef>();

        // Act
        var systemUnderTest = new XmlSectionFactory(mockReadAtf, null, mockSection, mockInforef);

        // Assert
        Assert.That(systemUnderTest, Is.Not.Null);
        Assert.That(systemUnderTest.SectionsSectionXmlSection, Is.EqualTo(mockSection));
        Assert.That(systemUnderTest.SectionsInforefXmlInforef, Is.EqualTo(mockInforef));
        Assert.That(systemUnderTest.CurrentTime, Is.Not.Empty);
    }

    [Test]
    // ANF-ID: [GHO11]
    public void XmlSectionFactory_CreateSectionFactory_SectionFoldersCreatedAndInforefSectionSerialized()
    {
        // Arrange
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();
        var mockSection = Substitute.For<ISectionsSectionXmlSection>();
        var mockInforef = Substitute.For<ISectionsInforefXmlInforef>();
        mockSection.PluginLocalAdlerSection = Substitute.For<SectionsSectionXmlPluginLocalAdlerSection>();
        mockSection.PluginLocalAdlerSection.AdlerSection = Substitute.For<SectionsSectionXmlAdlerSection>();
        var currWorkDir = ApplicationPaths.BackupFolder;

        var mockContent = new List<int?>();
        mockContent.Add(1);
        var learningSpaceJson1 =
            new LearningSpaceJson(1, "", "s", mockContent, 0, "", "", requiredSpacesToEnter: "1");
        var learningSpaceJson2 =
            new LearningSpaceJson(2, "", "s", mockContent, 0, "", "", requiredSpacesToEnter: "2");

        var learningSpaceList = new List<ILearningSpaceJson>();
        learningSpaceList.Add(learningSpaceJson1);
        learningSpaceList.Add(learningSpaceJson2);

        mockReadAtf.GetSpaceList().Returns(learningSpaceList);
        mockReadAtf.GetBaseLearningElementsList().Returns(
            new List<IBaseLearningElementJson> { new BaseLearningElementJson(3, "", "baseElement", "", "h5p", "h5p") });

        // Act
        var systemUnderTest = new XmlSectionFactory(mockReadAtf, mockFileSystem, mockSection, mockInforef);
        systemUnderTest.CreateSectionFactory();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceJsons, Is.EqualTo(learningSpaceList));
            Assert.That(
                mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections",
                    "section_0")), Is.True);
            Assert.That(
                mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections",
                    "section_" + learningSpaceJson1.SpaceId)), Is.True);
            Assert.That(
                mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections",
                    "section_" + learningSpaceJson2.SpaceId)), Is.True);
            Assert.That(
                mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections",
                    "section_" + (learningSpaceList.Count + 1))), Is.True);
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Id,
                Is.EqualTo((learningSpaceJson2.SpaceId + 1).ToString()));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Number,
                Is.EqualTo((learningSpaceJson2.SpaceId + 1).ToString()));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Name, Is.EqualTo("Hinweise auf externe Lerninhalte"));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Timemodified,
                Is.Not.Empty);

            mockInforef.Received().Serialize("", "0");
            mockInforef.Received().Serialize("", learningSpaceJson1.SpaceId.ToString());
            mockInforef.Received().Serialize("", learningSpaceJson2.SpaceId.ToString());
            mockInforef.Received().Serialize("", (learningSpaceJson2.SpaceId + 1).ToString());
            mockSection.Received().Serialize("", "0");
            mockSection.Received().Serialize("", learningSpaceJson1.SpaceId.ToString());
            mockSection.Received().Serialize("", learningSpaceJson2.SpaceId.ToString());
            mockSection.Received().Serialize("", (learningSpaceJson2.SpaceId + 1).ToString());
        });
    }
}