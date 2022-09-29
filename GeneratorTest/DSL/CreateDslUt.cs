using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using NUnit.Framework;
using PersistEntities;

namespace GeneratorTest.DSL;

[TestFixture]
public class CreateDslUt
{
    [Test]
    public void CreateDSL_WriteLearningWorld_DSLDocumentWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        var content1 = new LearningContentPe("a", "h5p", new byte[]{0x01,0x02});
        var ele3 = new LearningElementPe("a", "b",content1, "pupup", "g","h", LearningElementDifficultyEnumPe.Easy, 17, 2, 23);
        var space1 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff", 5);
        space1.LearningElements.Add(ele3);
        var space2 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff", 5);
        var learningSpaces = new List<LearningSpacePe> { space1, space2 };

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals,
             learningSpaces);

        var systemUnderTest = new CreateDsl(mockFileSystem);
        
        var learningElementsList = new List<LearningElementPe> { ele3 };

        //Act
        systemUnderTest.WriteLearningWorld(learningWorld);
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "DSL_Document.json");
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Uuid, Is.Not.Null);
            Assert.That(systemUnderTest.LearningWorldJson, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldJson!.Identifier.Value, Is.EqualTo(name));

            Assert.That(systemUnderTest.ListLearningElements, Is.EquivalentTo(learningElementsList));
            
            Assert.That(systemUnderTest.ListLearningSpaces, Has.Count.EqualTo(2));
        });
        Assert.Multiple(() =>
        {
            Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
        });
    }
}