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

        var identifierLearningWorldJson = new LmsElementIdentifierJson("name", "World");

        var identifierLearningSpaceJson1 = new LmsElementIdentifierJson("name", "Space_1");

        var identifierLearningSpaceJson2 = new LmsElementIdentifierJson("name", "Space_2");

        var identifierLearningElementJson1 = new LmsElementIdentifierJson("name", "Element_1");

        var identifierLearningElementJson2 = new LmsElementIdentifierJson("name", "DSL Dokument");

        var topicsJson = new TopicJson(1, "A", new List<int> { 1, 2 });
        var topicsList = new List<TopicJson>() { topicsJson };

        var learningSpacesJson1 = new LearningSpaceJson(1, identifierLearningSpaceJson1,
            "space1", new List<int> { 1, 2 }, 0, "spacedescription1", new[] { "spacegoals1" });

        var learningSpacesJson2 = new LearningSpaceJson(2, identifierLearningSpaceJson2, "space2",
            new List<int>(), 0, "spacedescription2", new[] { "spacegoals2" });

        var learningSpacesList = new List<LearningSpaceJson>() { learningSpacesJson1, learningSpacesJson2 };

        var learningElementJson1 = new LearningElementJson(1,
            identifierLearningElementJson1, "element1", "", "","h5p", 1, 1);

        var learningElementJson2 = new LearningElementJson(2,
            identifierLearningElementJson2, "element2", "", "","json", 1, 2);

        var learningElementJson3 = new LearningElementJson(3,
            identifierLearningElementJson2, "element3", "", "","url",  1, 3);

        var learningElementJson4 = new LearningElementJson(4,
            identifierLearningElementJson2, "element4", "", "","label", 1, 4);

        var learningElementList = new List<LearningElementJson>()
            { learningElementJson1, learningElementJson2, learningElementJson3, learningElementJson4 };

        var learningWorldJson = new LearningWorldJson(identifierLearningWorldJson, "world",
            topicsList, learningSpacesList, learningElementList, "World Description", new[] { "World Goals"});

        var rootJson = new DocumentRootJson("0.3","0.3.2", "marvin", "de", learningWorldJson);
        

        //Act
        var systemUnderTest = new ReadDsl(mockFileSystem);
        systemUnderTest.ReadLearningWorld("dslPath", rootJson);

        var listSpace = systemUnderTest.GetSectionList();
        var resourceList = systemUnderTest.GetResourceList();

        //Assert
        var getLearningWorldJson = systemUnderTest.GetLearningWorld();
        var getH5PElementsList = systemUnderTest.GetH5PElementsList();
        var elementsOrderedList = systemUnderTest.GetElementsOrderedList();
        var getLabelsList = systemUnderTest.GetLabelsList();
        var getUrlList = systemUnderTest.GetUrlList();

        Assert.Multiple(() =>
        {
            Assert.That(learningWorldJson, Is.Not.Null);
            Assert.That(getLearningWorldJson.Elements, Is.Not.Null);
            Assert.That(getLearningWorldJson.Spaces, Is.Not.Null);
            
            Assert.That(getLearningWorldJson.Elements, Has.Count.EqualTo(learningElementList.Count));
            Assert.That(getLearningWorldJson.Spaces, Has.Count.EqualTo(learningSpacesList.Count));
            Assert.That(resourceList, Has.Count.EqualTo(1));
            Assert.That(getH5PElementsList, Has.Count.EqualTo(1));
            
            //Elements + World Description & Goals (As they are created as Labels in Moodle)
            Assert.That(elementsOrderedList, Has.Count.EqualTo(3));
            Assert.That(getLabelsList, Has.Count.EqualTo(1));
            
            Assert.That(listSpace.Count, Is.EqualTo(3));
            
            //Currently all elements with Element-Type "mp4" are added to the list of Urls
            Assert.That(getUrlList, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void ReadDSL_ReadLearningWorld_DSLDocumentReadWithEmptyWorldDescriptionAndGoals()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();

        var identifierLearningWorldJson = new LmsElementIdentifierJson("name", "World");
        var topicsJson = new TopicJson(1, "A", 
            new List<int> {1, 2});
        var topicsList = new List<TopicJson>(){topicsJson};
        
     
        var learningWorldJson = new LearningWorldJson( identifierLearningWorldJson, "world",
            topicsList, new List<LearningSpaceJson>(), 
            new List<LearningElementJson>(), "", new [] { "" });

        var rootJson = new DocumentRootJson("0.3","0.3.2", "marvin", "de",learningWorldJson);
        

        //Act
        var systemUnderTest = new ReadDsl(mockFileSystem);
        systemUnderTest.ReadLearningWorld("dslPath", rootJson);

        var listSpace = systemUnderTest.GetSectionList();

        //Assert
        var getLearningWorldJson = systemUnderTest.GetLearningWorld();
        var getSpacesAndElementsList = systemUnderTest.GetElementsOrderedList();

        Assert.Multiple(() =>
        {
            Assert.That(learningWorldJson, Is.Not.Null);
            Assert.That(getLearningWorldJson.Elements, Is.Not.Null);
            Assert.That(getLearningWorldJson.Spaces, Is.Not.Null);

            //If Description and Goals are empty, they are not added to the list of Labels
            //But 
            Assert.That(getSpacesAndElementsList, Has.Count.EqualTo(0));

            //There is always at least 1 Section (Topic 0) in a Moodle Course
            Assert.That(listSpace.Count, Is.EqualTo(1));

        });
    }

    
}