using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
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