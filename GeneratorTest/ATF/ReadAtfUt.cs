using System.IO.Abstractions.TestingHelpers;
using Generator.ATF;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.ATF;

[TestFixture]
public class ReadAtfUt
{
    [Test]
    public void ReadATF_ReadLearningWorld_ATFDocumentRead()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<ReadAtf>>();

        var topicsJson = new TopicJson(1, "A", new List<int> { 1, 2 });
        var topicsList = new List<ITopicJson> { topicsJson };

        var learningSpacesJson1 = new LearningSpaceJson(1, "",
            "space1", new List<int?> { 1, 2 }, 0, "spacedescription1", "", "", new[] { "spacegoals1" });

        var learningSpacesJson2 = new LearningSpaceJson(2, "", "space2",
            new List<int?>(), 0, "", "", "", new[] { "spacegoals2" });

        var learningSpacesList = new List<ILearningSpaceJson> { learningSpacesJson1, learningSpacesJson2 };

        var learningElementJson1 = new LearningElementJson(1,
            "", "element1", "", "", "h5p", 1, 1, "");

        var learningElementJson2 = new LearningElementJson(2,
            "", "element2", "", "", "json", 1, 2, "");

        var learningElementJson3 = new LearningElementJson(3,
            "", "element3", "", "", "url", 1, 3, "");

        var learningElementJson4 = new LearningElementJson(4,
            "", "element4", "", "", "label", 1, 4, "");

        var learningElementList = new List<IElementJson>
            { learningElementJson1, learningElementJson2, learningElementJson3, learningElementJson4 };

        var learningWorldJson = new LearningWorldJson("world", "",
            topicsList, learningSpacesList, learningElementList, "World Description", new[] { "World Goals" });

        var rootJson = new DocumentRootJson("0.3", "0.3.2", "marvin", "de", learningWorldJson);


        //Act
        var systemUnderTest = new ReadAtf(mockFileSystem, mockLogger);
        systemUnderTest.ReadLearningWorld("atfPath", rootJson);

        var listSpace = systemUnderTest.GetSpaceList();
        var resourceList = systemUnderTest.GetResourceElementList();

        //Assert
        var getLearningWorldJson = systemUnderTest.GetLearningWorld();
        var getH5PElementsList = systemUnderTest.GetH5PElementsList();
        var elementsOrderedList = systemUnderTest.GetElementsOrderedList();
        var getWorldAttributes = systemUnderTest.GetWorldAttributes();
        var getUrlList = systemUnderTest.GetUrlElementList();

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
            Assert.That(getWorldAttributes.ElementName, Is.EqualTo("World Description"));
            Assert.That(getWorldAttributes.ElementDescription, Is.EqualTo("World Description"));
            Assert.That(getWorldAttributes.ElementMaxScore, Is.EqualTo(0));
            Assert.That(getWorldAttributes.LearningSpaceParentId, Is.EqualTo(0));
            Assert.That(getWorldAttributes.ElementFileType, Is.EqualTo("label"));
            Assert.That(getWorldAttributes.Url, Is.EqualTo(""));
            Assert.That(getWorldAttributes.ElementCategory, Is.EqualTo("World Attributes"));
            Assert.That(getWorldAttributes.ElementId, Is.EqualTo(5));
            Assert.That(getWorldAttributes.ElementUUID, Is.EqualTo(""));
            Assert.That(getWorldAttributes.ElementModel, Is.EqualTo(""));

            Assert.That(listSpace.Count, Is.EqualTo(2));

            //Currently all elements with Element-Type "mp4" are added to the list of Urls
            Assert.That(getUrlList, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void ReadATF_ReadLearningWorld_ATFDocumentReadWithEmptyWorldDescriptionAndGoals()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var mockLogger = Substitute.For<ILogger<ReadAtf>>();

        var topicsJson = new TopicJson(1, "A",
            new List<int> { 1, 2 });
        var topicsList = new List<ITopicJson> { topicsJson };


        var learningWorldJson = new LearningWorldJson("world", "",
            topicsList, new List<ILearningSpaceJson>(),
            new List<IElementJson>(), "", new[] { "" });

        var rootJson = new DocumentRootJson("0.3", "0.3.2", "marvin", "de", learningWorldJson);


        //Act
        var systemUnderTest = new ReadAtf(mockFileSystem, mockLogger);
        systemUnderTest.ReadLearningWorld("atfPath", rootJson);

        var listSpace = systemUnderTest.GetSpaceList();

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
            Assert.That(listSpace.Count, Is.EqualTo(0));
        });
    }
}