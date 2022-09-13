using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using NUnit.Framework;
using PersistEntities;

namespace GeneratorTest.DSL;

[TestFixture]
public class ReadDslUt
{
    
    [Test]
    public void ReadDSL_ReadLearningWorld_DSLDocumentRead()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        var content1 = new LearningContentPe("a", "h5p", new byte[] {0x01, 0x02});
        var ele3 = new LearningElementPe("a", "b", content1, "pupup", "g", "h", LearningElementDifficultyEnumPe.Easy, 17, 23, 23);
        var space1 = new LearningSpacePe("ff", "ff", "ff", "ff", "ff");
        space1.LearningElements.Add(ele3);

        var identifierLearningWorldJson = new IdentifierJson("name", "World");

        var identifierLearningSpaceJson1 = new IdentifierJson("name", "Space_1");
  
        var identifierLearningSpaceJson2 = new IdentifierJson("name", "Space_2");
    
        var identifierLearningElementJson1 = new IdentifierJson("name", "Element_1");
   
        var identifierLearningElementJson2 = new IdentifierJson("name", "DSL Dokument");

        var learningElementValueJson1 = new LearningElementValueJson("text", "Hello World");

        var learningElementValueJson2 = new LearningElementValueJson("text", "Hello Space");

        var learningElementValueList1 = new List<LearningElementValueJson>(){learningElementValueJson1};
        var learningElementValueList2 = new List<LearningElementValueJson>(){learningElementValueJson2};
        
        var learningWorldContentJson = new List<int>(){1,2};
        
        var topicsJson = new TopicJson();
        var topicsList = new List<TopicJson>(){topicsJson};

        var learningSpacesJson1 = new LearningSpaceJson(1, "Space_1",
            identifierLearningSpaceJson1, new List<int>() {1, 2});
 
        var learningSpacesJson2 = new LearningSpaceJson(1, "Space_1", 
            identifierLearningSpaceJson2, new List<int>() {3, 4});

        var learningSpacesList = new List<LearningSpaceJson>(){learningSpacesJson1, learningSpacesJson2};

        var learningElementJson1 = new LearningElementJson(1,
            identifierLearningElementJson1, "h5p",0);
        learningElementJson1.LearningElementValue = learningElementValueList1;

        var learningElementJson2 = new LearningElementJson(2,
            identifierLearningElementJson2, "json",0);
        learningElementJson2.LearningElementValue = learningElementValueList2;

        var learningElementList = new List<LearningElementJson>(){learningElementJson1, learningElementJson2};
        
        var learningWorldJson = new LearningWorldJson("uuid", identifierLearningWorldJson, learningWorldContentJson, topicsList, learningSpacesList, learningElementList);

        var rootJson = new DocumentRootJson(learningWorldJson);
        

        //Act
        var systemUnderTest = new ReadDsl(mockFileSystem);
        systemUnderTest.ReadLearningWorld("dslPath", rootJson);

        var listSpace = systemUnderTest.GetLearningSpaceList();
        var resourceList = systemUnderTest.GetResourceList();

        //Assert
        var getLearningWorldJson = systemUnderTest.GetLearningWorld();
        var getH5PElementsList = systemUnderTest.GetH5PElementsList();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ListH5PElements, Is.Not.Null);
            Assert.That(learningWorldJson, Is.Not.Null);
            Assert.That(systemUnderTest.ListH5PElements, Has.Count.EqualTo(1));
            Assert.That(getLearningWorldJson.LearningElements, Is.Not.Null);
            Assert.That(getLearningWorldJson.LearningSpaces, Is.Not.Null);
            Assert.That(getLearningWorldJson.LearningElements, Has.Count.EqualTo(learningElementList.Count));
            Assert.That(getLearningWorldJson.LearningSpaces, Has.Count.EqualTo(learningSpacesList.Count));
            Assert.That(resourceList, Has.Count.EqualTo(1));
            Assert.That(getH5PElementsList, Has.Count.EqualTo(1));
            Assert.That(listSpace.Count, Is.EqualTo(2));
        });
    }

    
}