using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.PersistEntities;
using Generator.DSL;
using Generator.PersistEntities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class ReadDslUt
{
    
    [Test]
    public void ReadDSL_ReadLearningWorld_DSLDocumentRead()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        var content1 = new LearningContentPe("a", "h5p", new byte[] {0x01, 0x02});
        var content2 = new LearningContentPe("w", ".h5p", new byte[] {0x02, 0x01});
        var ele1 = new LearningElementPe("a", "b", "e", content1, "pupup", "g", "h", LearningElementDifficultyEnumPe.Easy, 17, 23);
        var ele2 = new LearningElementPe("z", "zz", "zzz", content2, "baba", "z", "zz", LearningElementDifficultyEnumPe.Easy, 444, double.MaxValue);
        var ele3 = new LearningElementPe("a", "b", "e", content1, "pupup", "g", "h", LearningElementDifficultyEnumPe.Easy, 17, 23);
        var learningElements = new List<LearningElementPe> {ele1, ele2};
        var space1 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff");
        space1.LearningElements.Add(ele3);
        var space2 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<LearningSpacePe> {space1, space2};

        var learningWorld = new LearningWorldPe(name, shortname, authors, language, description, goals,
            learningElements, learningSpaces);
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
            identifierLearningElementJson_1, "h5p");
        learningElementJson_1.LearningElementValue = learningElementValueList_1;

        var learningElementJson_2 = new LearningElementJson(2,
            identifierLearningElementJson_2, "json");
        learningElementJson_2.LearningElementValue = learningElementValueList_2;

        var learningElementList = new List<LearningElementJson>(){learningElementJson_1, learningElementJson_2};
        
        var learningWorldJson = new LearningWorldJson(identifierLearningWorldJson, learningWorldContentJson, topicsList, learningSpacesList, learningElementList);

        var rootJson = new DocumentRootJson(learningWorldJson);
        

        //Act
        var systemUnderTest = new ReadDsl(mockFileSystem);
        systemUnderTest.ReadLearningWorld("dslPath", rootJson);

        var listSpace = systemUnderTest.GetLearningSpaceList();
        var listDslDocument = systemUnderTest.GetDslDocumentList();

        //Assert
        var getLearningWorldJson = systemUnderTest.GetLearningWorld();
        var getH5PElementsList = systemUnderTest.GetH5PElementsList();
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListH5PElements, Is.Not.Null);
            //Assert.That(h5PElementsList, Is.Not.Null);
            Assert.That(learningWorldJson, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListH5PElements!, Has.Count.EqualTo(1));
            Assert.That(getLearningWorldJson!.LearningElements, Is.Not.Null);
            Assert.That(getLearningWorldJson.LearningSpaces, Is.Not.Null);
            Assert.That(getLearningWorldJson.LearningElements!, Has.Count.EqualTo(learningElementList.Count));
            Assert.That(getLearningWorldJson.LearningSpaces!, Has.Count.EqualTo(learningSpacesList.Count));
            Assert.That(getH5PElementsList!, Has.Count.EqualTo(1));
            Assert.That(listSpace.Count, Is.EqualTo(2));
            Assert.That(listDslDocument.Count, Is.EqualTo(1));
        });
    }

    
}