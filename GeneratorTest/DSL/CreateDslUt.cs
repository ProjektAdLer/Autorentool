using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;

namespace GeneratorTest.DSL;

[TestFixture]
public class CreateDslUt
{

    [Test]
    public void CreateDSL_SearchDuplicateLearningElementNames_DuplicatesFoundAndNamesChanged()
    {
        //Arrange
        var mockElement1 = new LearningElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.LearningElementDifficultyEnumPe.Easy);
        var mockElement2 = new LearningElementPe("Another Element", "el", null, "", 
            "", "", "", PersistEntities.LearningElementDifficultyEnumPe.Easy);
        var mockElement3 = new LearningElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.LearningElementDifficultyEnumPe.Easy);
        var mockElement4 = new LearningElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.LearningElementDifficultyEnumPe.Easy);
        var mockElement5 = new LearningElementPe("Same Name Element", "el", null, "", 
            "", "", "", PersistEntities.LearningElementDifficultyEnumPe.Easy);
        
        var mockLearningElements1 = new List<LearningElementPe> {mockElement1, mockElement2};
        var mockLearningElements2 = new List<LearningElementPe> {mockElement3};
        var mockLearningElements3 = new List<LearningElementPe> {mockElement4, mockElement5};

        var mockSpace1 = new LearningSpacePe("Space1", "sp", null, "", "", 1,
            mockLearningElements1);
        var mockSpace2 = new LearningSpacePe("Space2", "sp", null, "", "", 1,
            mockLearningElements2);
        var mockSpace3 = new LearningSpacePe("Space3", "sp", null, "", "", 1,
            mockLearningElements3);
        
        
        var mockSpaces = new List<LearningSpacePe> {mockSpace1, mockSpace2, mockSpace3};
        
        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();

        //Act
        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        var learningSpaceList = systemUnderTest.SearchDuplicateLearningElementNames(mockSpaces);

        //Assert
        Assert.Multiple(()=>{ 
            Assert.That(mockElement1.Name, Is.EqualTo("Same Name Element(1)"));
            Assert.That(mockElement2.Name, Is.EqualTo("Another Element"));
            Assert.That(mockElement3.Name, Is.EqualTo("Same Name Element(2)"));
            Assert.That(mockElement4.Name, Is.EqualTo("Same Name Element(3)"));
            Assert.That(mockElement5.Name, Is.EqualTo("Same Name Element(4)"));
            Assert.That(learningSpaceList.Count, Is.EqualTo(3));
            Assert.That(learningSpaceList[0].LearningElements.Count, Is.EqualTo(2));
            Assert.That(learningSpaceList[1].LearningElements.Count, Is.EqualTo(1));
            Assert.That(learningSpaceList[2].LearningElements.Count, Is.EqualTo(2));
        });
    }
    
    [Test]
    public void CreateDSL_WriteLearningWorld_DSLDocumentWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/bar.txt", new MockFileData("barbaz"));
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(curWorkDir, "XMLFilesForExport"));
        mockFileSystem.AddFile(curWorkDir + "\\XMLFilesForExport\\LearningWorld.xml", new MockFileData(""));
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();
        
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        
        var content1 = new LearningContentPe("FileName", "h5p", "/foo/bar.txt");
        var content2 = new LearningContentPe("FileName", "png", "/foo/bar.txt");
        var content3 = new LearningContentPe("FileName", "url", "/foo/bar.txt");
        var content4 = new LearningContentPe("FileName", "txt", "/foo/foo.txt");
        var content5 = new LearningContentPe("FileName", "pdf", "/foo/foo.txt");

        var ele1 = new LearningElementPe("a", "b",content1, "", "pupup", "g","h", 
            LearningElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele2 = new LearningElementPe("b", "b",content2, "", "pupup", "g","h", 
            LearningElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele3 = new LearningElementPe("c", "b", content3, "","pupup", "g","h", 
            LearningElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele4 = new LearningElementPe("d", "b",content4, "","pupup", "g","h", 
            LearningElementDifficultyEnumPe.Easy, 17, 2, 23);
        var ele5 = new LearningElementPe("e", "b",content5, "","pupup", "g","h", 
            LearningElementDifficultyEnumPe.Easy, 17, 2, 23);
        

        var space1 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff", 5, 
            null, 0, 0, new List<LearningSpacePe>(), 
            new List<LearningSpacePe>());
        space1.LearningElements.AddRange(new List<LearningElementPe>{ele1, ele2, ele3, ele4, ele5});
        var space2 = new LearningSpacePe("ff2", "ff", "ff", "ff", "ff", 5, 
            null, 0, 0, new List<LearningSpacePe>(), new List<LearningSpacePe>());
        space1.OutBoundSpaces = new List<LearningSpacePe>() {space2};
        space2.InBoundSpaces = new List<LearningSpacePe>() {space1};
        var learningSpaces = new List<LearningSpacePe> { space1, space2 };

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals,
             learningSpaces);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);
        
        //Every Element except Content with "url" is added to the comparison list.
        var learningElementsSpace1 = new List<LearningElementPe> { ele1, ele2, ele4, ele5 };
        var learningElementsSpace2 = new List<LearningElementPe>();
        
        var learningElementsForComparison = new List<List<LearningElementPe>> {learningElementsSpace1, learningElementsSpace2};
        

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
            Assert.That(systemUnderTest.ListLearningSpaces, Is.EquivalentTo(learningSpaces));
            Assert.That(systemUnderTest.LearningWorldJson.LearningSpaces[0].Requirements,
                Is.EqualTo(new List<int>()));
            Assert.That(systemUnderTest.LearningWorldJson.LearningSpaces[1].Requirements,
                Is.EqualTo(new List<int>() {1}));
        });
        Assert.Multiple(() =>
        {
            Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
        });
    }
    
    [Test]
     public void CreateDSL_WriteLearningWorld_UnsupportedTypeExceptionThrown()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile("/foo/foo.txt", new MockFileData("foo"));
        var mockLogger = Substitute.For<ILogger<CreateDsl>>();
        
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        
        var content1 = new LearningContentPe("FileName", "mp3", "/foo/bar.txt");

        var ele1 = new LearningElementPe("a", "b",content1, "", "pupup", "g","h", 
            LearningElementDifficultyEnumPe.Easy, 17, 2, 23);

        var space1 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff", 5, 
            null, 0, 0, new List<LearningSpacePe>(), 
            new List<LearningSpacePe>());
        space1.LearningElements.AddRange(new List<LearningElementPe>{ele1});
        var learningSpaces = new List<LearningSpacePe> { space1 };

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals,
             learningSpaces);

        var systemUnderTest = new CreateDsl(mockFileSystem, mockLogger);

        //Act
        try
        {
            systemUnderTest.WriteLearningWorld(learningWorld); 
            Assert.Fail("Learning Content Exception was not thrown");
        }
        catch (Exception e)
        {
            //Assert
            Assert.That(e.Message, Is.EqualTo("The given LearningContent Type is not supported - in CreateDsl."));
        }

    }
    
}