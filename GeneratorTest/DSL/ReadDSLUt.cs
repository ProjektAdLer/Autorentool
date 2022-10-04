using System.IO.Abstractions.TestingHelpers;
using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class ReadDslUt
{
    
    [Test]
    public void ReadDSL_ReadLearningWorld_DSLDocumentRead()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        var identifierLearningWorldJson = new IdentifierJson("name", "World");

        var identifierLearningSpaceJson1 = new IdentifierJson("name", "Space_1");
  
        var identifierLearningSpaceJson2 = new IdentifierJson("name", "Space_2");
    
        var identifierLearningElementJson1 = new IdentifierJson("name", "Element_1");
   
        var identifierLearningElementJson2 = new IdentifierJson("name", "DSL Dokument");

        var learningElementValueJson1 = new LearningElementValueJson("points", 0);

        var learningElementValueJson2 = new LearningElementValueJson("points", 0);

        var learningElementValueList1 = new List<LearningElementValueJson>(){learningElementValueJson1};
        var learningElementValueList2 = new List<LearningElementValueJson>(){learningElementValueJson2};
        
        var learningWorldContentJson = new List<int>(){1,2};
        
        var topicsJson = new TopicJson();
        var topicsList = new List<TopicJson>(){topicsJson};

        var learningSpacesJson1 = new LearningSpaceJson(1, identifierLearningSpaceJson1, 
            new List<int> {1, 2}, 0, 0);
 
        var learningSpacesJson2 = new LearningSpaceJson(2, identifierLearningSpaceJson2, 
            new List<int>(), 0, 0);

        var learningSpacesList = new List<LearningSpaceJson>(){learningSpacesJson1, learningSpacesJson2};

        var learningElementJson1 = new LearningElementJson(1,
            identifierLearningElementJson1, "", "", "h5p",1, learningElementValueList1);
        
        var learningElementJson2 = new LearningElementJson(2,
            identifierLearningElementJson2, "", "", "json",1, learningElementValueList2);
        
        var learningElementJson3 = new LearningElementJson(3,
            identifierLearningElementJson2, "", "", "mp4",1, learningElementValueList2);
        
        var learningElementJson4 = new LearningElementJson(4,
            identifierLearningElementJson2, "", "", "label",1, learningElementValueList2);

        var learningElementList = new List<LearningElementJson>(){learningElementJson1, learningElementJson2, learningElementJson3, learningElementJson4};
        
        var learningWorldJson = new LearningWorldJson("uuid", identifierLearningWorldJson, learningWorldContentJson, topicsList, learningSpacesList, learningElementList);

        var rootJson = new DocumentRootJson(learningWorldJson);
        

        //Act
        var systemUnderTest = new ReadDsl(mockFileSystem);
        systemUnderTest.ReadLearningWorld("dslPath", rootJson);

        var listSpace = systemUnderTest.GetSectionList();
        var resourceList = systemUnderTest.GetResourceList();

        //Assert
        var getLearningWorldJson = systemUnderTest.GetLearningWorld();
        var getH5PElementsList = systemUnderTest.GetH5PElementsList();
        var getSpacesAndElementsList = systemUnderTest.GetSpacesAndElementsOrderedList();
        var getLabelsList = systemUnderTest.GetLabelsList();
        var getUrlList = systemUnderTest.GetUrlList();

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
            
            //Spaces + Elements 
            Assert.That(getSpacesAndElementsList, Has.Count.EqualTo(4));
            Assert.That(getLabelsList, Has.Count.EqualTo(1));
            
            //Because there are no Topics in the AuthoringTool, every learning space is added to Topic 0
            Assert.That(listSpace.Count, Is.EqualTo(1));
            
            //Currently all elements with Element-Type "mp4" are added to the list of Urls
            Assert.That(getUrlList, Has.Count.EqualTo(1));
        });
    }

    
}