using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses;

[TestFixture]
public class XmlSectionFactoryUt
{
    [Test]
    public void XmlSectionFactory_StandardConstructor_AllPropertiesAreSet()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockSection = Substitute.For<ISectionsSectionXmlSection>();
        var mockInforef = Substitute.For<ISectionsInforefXmlInforef>();
        
        // Act
        var systemUnderTest = new XmlSectionFactory(mockReadDsl, null, mockSection, mockInforef);

        // Assert
        Assert.That(systemUnderTest, Is.Not.Null);
        Assert.That(systemUnderTest.SectionsSectionXmlSection, Is.EqualTo(mockSection));
        Assert.That(systemUnderTest.SectionsInforefXmlInforef, Is.EqualTo(mockInforef));
        Assert.That(systemUnderTest.CurrentTime, Is.Not.Null);
    }

    [Test]
    public void XmlSectionFactory_CreateSectionFactory_SectionFoldersCreatedAndInforefSectionSerialized()
    {
        // Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockFileSystem = new MockFileSystem();
        var mockSection = Substitute.For<ISectionsSectionXmlSection>();
        var mockInforef = Substitute.For<ISectionsInforefXmlInforef>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        var mockIdentifierJson = new IdentifierJson("TestValue", "TestType");

        var mockContent = new List<int>();
        mockContent.Add(1);
        var learningSpaceJson1 = new LearningSpaceJson(1, mockIdentifierJson, mockContent);
        var learningSpaceJson2 = new LearningSpaceJson(2, mockIdentifierJson, mockContent);

        var learningSpaceList = new List<LearningSpaceJson>();
        learningSpaceList.Add(learningSpaceJson1);
        learningSpaceList.Add(learningSpaceJson2);

        mockReadDsl.GetLearningSpaceList().Returns(learningSpaceList);
        
        // Act
        var systemUnderTest = new XmlSectionFactory(mockReadDsl, mockFileSystem, mockSection, mockInforef);
        systemUnderTest.CreateSectionFactory();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningSpaceJsons, Is.EqualTo(learningSpaceList));
            Assert.That(mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_"+learningSpaceJson1.SpaceId)), Is.True);
            Assert.That(mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_"+learningSpaceJson2.SpaceId)), Is.True);
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Id, Is.EqualTo(learningSpaceJson2.SpaceId.ToString()));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Number, Is.EqualTo(learningSpaceJson2.SpaceId.ToString()));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Name, Is.EqualTo(learningSpaceJson2.Identifier.Value));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Timemodified, Is.EqualTo(systemUnderTest.CurrentTime));
            
            mockInforef.Received().Serialize("", learningSpaceJson1.SpaceId.ToString());
            mockInforef.Received().Serialize("", learningSpaceJson2.SpaceId.ToString());
            mockSection.Received().Serialize("", learningSpaceJson1.SpaceId.ToString());
            mockSection.Received().Serialize("", learningSpaceJson2.SpaceId.ToString());
        });

    }
    
}