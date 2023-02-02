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
        var spaceJson1 = new SpaceJson(1, mockIdentifierJson, mockContent, 0, 0, "");
        var spaceJson2 = new SpaceJson(2, mockIdentifierJson, mockContent, 0, 0,"");

        var spaceList = new List<SpaceJson>();
        spaceList.Add(spaceJson1);
        spaceList.Add(spaceJson2);

        mockReadDsl.GetSectionList().Returns(spaceList);
        
        // Act
        var systemUnderTest = new XmlSectionFactory(mockReadDsl, mockFileSystem, mockSection, mockInforef);
        systemUnderTest.CreateSectionFactory();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SpaceJsons, Is.EqualTo(spaceList));
            Assert.That(mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_"+spaceJson1.SpaceId)), Is.True);
            Assert.That(mockFileSystem.Directory.Exists(Path.Join(currWorkDir, "XMLFilesForExport", "sections", "section_"+spaceJson2.SpaceId)), Is.True);
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Id, Is.EqualTo(spaceJson2.SpaceId.ToString()));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Number, Is.EqualTo(spaceJson2.SpaceId.ToString()));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Name, Is.EqualTo(spaceJson2.Identifier.Value));
            Assert.That(systemUnderTest.SectionsSectionXmlSection.Timemodified, Is.EqualTo(systemUnderTest.CurrentTime));
            
            mockInforef.Received().Serialize("", spaceJson1.SpaceId.ToString());
            mockInforef.Received().Serialize("", spaceJson2.SpaceId.ToString());
            mockSection.Received().Serialize("", spaceJson1.SpaceId.ToString());
            mockSection.Received().Serialize("", spaceJson2.SpaceId.ToString());
        });

    }
    
}