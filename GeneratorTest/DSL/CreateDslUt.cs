﻿using System.IO.Abstractions.TestingHelpers;
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
        mockFileSystem.AddFile("/foo/bar.txt", new MockFileData("barbaz"));
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        var content1 = new LearningContentPe("a", "h5p", "/foo/bar.txt");
        var content2 = new LearningContentPe("w", "e", "/foo/foo.txt");
        var ele1 = new LearningElementPe("a", "b",content1, "pupup", "g","h", LearningElementDifficultyEnumPe.Easy, 17, 1,23);
        var ele2 = new LearningElementPe("z", "zz", content2,"baba", "z","zz", LearningElementDifficultyEnumPe.Easy, 444, 3,double.MaxValue);
        var ele3 = new LearningElementPe("a(1)", "b",content1, "pupup", "g","h", LearningElementDifficultyEnumPe.Easy, 17, 2, 23);
        var learningElements = new List<LearningElementPe> { ele1, ele2 };
        var space1 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff", 5);
        space1.LearningElements.Add(ele3);
        var space2 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff", 5);
        var learningSpaces = new List<LearningSpacePe> { space1, space2 };

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals,
            learningElements, learningSpaces);

        var systemUnderTest = new CreateDsl(mockFileSystem);
        
        var allLearningElements = new List<LearningElementPe> { ele3, ele1, ele2 };

        //Act
        systemUnderTest.WriteLearningWorld(learningWorld);
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "DSL_Document");
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Uuid, Is.Not.Null);
            Assert.That(systemUnderTest.LearningWorldJson, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorldJson!.Identifier.Value, Is.EqualTo(name));

            Assert.That(systemUnderTest.ListLearningElements, Is.EquivalentTo(allLearningElements));

            //Because SpaceId=0 is automatically created and has all elements free in the learning world.
            //The Space count is 2 + 1
            Assert.That(systemUnderTest.ListLearningSpaces, Has.Count.EqualTo(3));
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListLearningSpaces[0].Name, Is.EqualTo("Freie Lernelemente"));
            Assert.That(systemUnderTest.LearningWorldJson!.LearningElements[0].Identifier.Value, Is.EqualTo("DSL_Document"));

            Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
        });
    }
}