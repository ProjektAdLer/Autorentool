using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using DataAccess.Persistence;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using PersistEntities.LearningContent;
using PersistEntities.LearningContent.Question;
using Shared;
using TestHelpers;

namespace DataAccessTest.Persistence;

/// <summary>
/// Component tests to test whether Persistence roundtrip produces equal objects
/// </summary>
[TestFixture]
public class PersistenceCt
{
    private const string FilePath = "awesomefile.txt";

    [Test]
    public void Persistence_SaveAndLoadWorld_Stream_ObjectsAreEquivalent()
    {
        var world = PersistEntityProvider.GetLearningWorld();
        var initialWorldId = world.Id;
        var topic = PersistEntityProvider.GetTopic();
        var initialTopicId = topic.Id;
        var space1 = PersistEntityProvider.GetLearningSpace();
        var initialSpace1Id = space1.Id;
        var space2 = PersistEntityProvider.GetLearningSpace();
        var initialSpace2Id = space2.Id;
        var condition1 = PersistEntityProvider.GetPathWayCondition();
        var initialCondition1Id = condition1.Id;
        var condition2 = PersistEntityProvider.GetPathWayCondition();
        var initialCondition2Id = condition2.Id;
        var content = PersistEntityProvider.GetFileContent();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var initialElementId = element.Id;
        space1.LearningSpaceLayout.LearningElements[0] = element;
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.Topics.Add(topic);
        world.PathWayConditions.Add(condition1);
        world.PathWayConditions.Add(condition2);
        space1.OutBoundObjects.Add(condition1);
        condition1.InBoundObjects.Add(space1);
        condition1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(condition1);
        space2.OutBoundObjects.Add(condition2);
        condition2.InBoundObjects.Add(space2);
        world.LearningPathways.Add(PersistEntityProvider.GetLearningPathway(source: space1, target: condition1));
        world.LearningPathways.Add(PersistEntityProvider.GetLearningPathway(source: condition1, target: space2));
        world.LearningPathways.Add(PersistEntityProvider.GetLearningPathway(source: space2, target: condition2));
        space1.AssignedTopic = topic;
        space2.AssignedTopic = topic;

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces[0].AssignedTopic, Is.EqualTo(world.Topics[0]));
            Assert.That(world.LearningSpaces[1].AssignedTopic, Is.EqualTo(world.Topics[0]));
        });

        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>();

        systemUnderTest.SaveToStream(world, stream);
        stream.Position = 0;
        var restoredWorld = systemUnderTest.LoadFromStream(stream);

        restoredWorld.Should().BeEquivalentTo(world, options => options.IgnoringCyclicReferences()
            .Excluding(obj => obj.Id)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.Id)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.AssignedTopic)
            .For(obj => obj.Topics).Exclude(obj => obj.Id)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.Id)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.Id)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.LearningSpaces).For(obj => obj.LearningSpaceLayout.ContainedLearningElements)
            .Exclude(obj => obj.Id)
            .Excluding(obj => obj.LearningPathways)
            .Excluding(obj => obj.LearningSpaces[0].LearningSpaceLayout.LearningElements[0].Id)
        );

        Assert.Multiple(() =>
        {
            Assert.That(restoredWorld.LearningSpaces[0].OutBoundObjects,
                Does.Contain(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningSpaces[0].AssignedTopic, Is.EqualTo(restoredWorld.Topics[0]));
            Assert.That(restoredWorld.LearningSpaces[1].AssignedTopic, Is.EqualTo(restoredWorld.Topics[0]));
            Assert.That(restoredWorld.PathWayConditions[0].InBoundObjects,
                Does.Contain(restoredWorld.LearningSpaces[0]));
            Assert.That(restoredWorld.PathWayConditions[0].OutBoundObjects,
                Does.Contain(restoredWorld.LearningSpaces[1]));
            Assert.That(restoredWorld.LearningSpaces[1].InBoundObjects,
                Does.Contain(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningSpaces[1].OutBoundObjects,
                Does.Contain(restoredWorld.PathWayConditions[1]));
            Assert.That(restoredWorld.PathWayConditions[1].InBoundObjects,
                Does.Contain(restoredWorld.LearningSpaces[1]));

            Assert.That(restoredWorld.LearningPathways[0].SourceObject, Is.EqualTo(restoredWorld.LearningSpaces[0]));
            Assert.That(restoredWorld.LearningPathways[0].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningPathways[1].SourceObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningPathways[1].TargetObject, Is.EqualTo(restoredWorld.LearningSpaces[1]));
            Assert.That(restoredWorld.LearningPathways[2].SourceObject, Is.EqualTo(restoredWorld.LearningSpaces[1]));
            Assert.That(restoredWorld.LearningPathways[2].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[1]));

            Assert.That(initialWorldId, Is.Not.EqualTo(restoredWorld.Id));
            Assert.That(initialSpace1Id, Is.Not.EqualTo(restoredWorld.LearningSpaces.First().Id));
            Assert.That(initialSpace2Id, Is.Not.EqualTo(restoredWorld.LearningSpaces.Last().Id));
            Assert.That(initialTopicId, Is.Not.EqualTo(restoredWorld.Topics.First().Id));
            Assert.That(initialCondition1Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.First().Id));
            Assert.That(initialCondition2Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.Last().Id));
            Assert.That(restoredWorld.LearningPathways.Select(pw => pw.Id), Is.All.Not.EqualTo(Guid.Empty));
        });
    }

    [Test]
    public void Persistence_SaveAndLoadSpace_Stream_ObjectsAreEquivalent()
    {
        var space = PersistEntityProvider.GetLearningSpace();
        var initialSpaceId = space.Id;
        var content = PersistEntityProvider.GetFileContent();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var initialElementId = element.Id;
        space.LearningSpaceLayout.LearningElements[0] = element;

        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<LearningSpacePe>();

        systemUnderTest.SaveToStream(space, stream);
        stream.Position = 0;
        var restoredSpace = systemUnderTest.LoadFromStream(stream);

        restoredSpace.Should().BeEquivalentTo(space, options => options
            .Excluding(obj => obj.Id)
            .For(obj => obj.LearningSpaceLayout.ContainedLearningElements)
            .Exclude(obj => obj.Id)
            .Excluding(obj => obj.LearningSpaceLayout.LearningElements[0].Id)
        );
        Assert.That(initialSpaceId, Is.Not.EqualTo(restoredSpace.Id));
    }

    [Test]
    public void Persistence_SaveAndLoadElement_Stream_ObjectsAreEquivalent()
    {
        var content = PersistEntityProvider.GetFileContent();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var initialElementId = element.Id;

        using var stream = new MemoryStream();
        var systemUnderTest = CreateTestableFileSaveHandler<LearningElementPe>();

        systemUnderTest.SaveToStream(element, stream);
        stream.Position = 0;
        var restoredElement = systemUnderTest.LoadFromStream(stream);

        restoredElement.Should().BeEquivalentTo(element, options => options.Excluding(obj => obj.Id));
    }

    [Test]
    public void Persistence_SaveAndLoadWorld_File_ObjectsAreEquivalent()
    {
        var world = PersistEntityProvider.GetLearningWorld();
        var initialWorldId = world.Id;
        var topic = PersistEntityProvider.GetTopic();
        var space1 = PersistEntityProvider.GetLearningSpace(assignedTopic: topic);
        var initialTopicId = topic.Id;
        var initialSpace1Id = space1.Id;
        var space2 = PersistEntityProvider.GetLearningSpace(assignedTopic: topic);
        var initialSpace2Id = space2.Id;
        var condition1 = PersistEntityProvider.GetPathWayCondition(condition: ConditionEnum.Or);
        var initialCondition1Id = condition1.Id;
        var condition2 = PersistEntityProvider.GetPathWayCondition(condition: ConditionEnum.And);
        var initialCondition2Id = condition2.Id;
        var content = PersistEntityProvider.GetFileContent();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var initialElementId = element.Id;
        var unplacedContent = PersistEntityProvider.GetFileContent();
        var unplacedElement = PersistEntityProvider.GetLearningElement(content: unplacedContent);
        var initialUnplacedElementId = unplacedElement.Id;
        space1.LearningSpaceLayout.LearningElements[0] = element;
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.Topics.Add(topic);
        world.PathWayConditions.Add(condition1);
        world.PathWayConditions.Add(condition2);
        space1.OutBoundObjects.Add(condition1);
        condition1.InBoundObjects.Add(space1);
        condition1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(condition1);
        space2.OutBoundObjects.Add(condition2);
        condition2.InBoundObjects.Add(space2);
        world.LearningPathways.Add(PersistEntityProvider.GetLearningPathway(source: space1, target: condition1));
        world.LearningPathways.Add(PersistEntityProvider.GetLearningPathway(source: condition1, target: space2));
        world.LearningPathways.Add(PersistEntityProvider.GetLearningPathway(source: space2, target: condition2));
        world.UnplacedLearningElements.Add(unplacedElement);
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem: mockFileSystem);

        systemUnderTest.SaveToDisk(world, FilePath);
        var restoredWorld = systemUnderTest.LoadFromDisk(FilePath);

        restoredWorld.Should().BeEquivalentTo(world, options => options
            .IgnoringCyclicReferences()
            .Excluding(obj => obj.Id)
            .For(obj => obj.UnplacedLearningElements).Exclude(obj => obj.Id)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.Id)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.LearningSpaces).Exclude(obj => obj.AssignedTopic)
            .For(obj => obj.Topics).Exclude(obj => obj.Id)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.Id)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.PathWayConditions).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.Id)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.InBoundObjects)
            .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.OutBoundObjects)
            .For(obj => obj.LearningSpaces).For(obj => obj.LearningSpaceLayout.ContainedLearningElements)
            .Exclude(obj => obj.Id)
            .Excluding(obj => obj.LearningPathways)
            .Excluding(obj => obj.LearningSpaces[0].LearningSpaceLayout.LearningElements[0].Id)
        );
        Assert.Multiple(() =>
        {
            Assert.That(restoredWorld.LearningSpaces[0].OutBoundObjects,
                Does.Contain(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningSpaces[0].AssignedTopic, Is.EqualTo(restoredWorld.Topics[0]));
            Assert.That(restoredWorld.LearningSpaces[1].AssignedTopic, Is.EqualTo(restoredWorld.Topics[0]));
            Assert.That(restoredWorld.PathWayConditions[0].InBoundObjects,
                Does.Contain(restoredWorld.LearningSpaces[0]));
            Assert.That(restoredWorld.PathWayConditions[0].OutBoundObjects,
                Does.Contain(restoredWorld.LearningSpaces[1]));
            Assert.That(restoredWorld.LearningSpaces[1].InBoundObjects,
                Does.Contain(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningSpaces[1].OutBoundObjects,
                Does.Contain(restoredWorld.PathWayConditions[1]));
            Assert.That(restoredWorld.PathWayConditions[1].InBoundObjects,
                Does.Contain(restoredWorld.LearningSpaces[1]));

            Assert.That(restoredWorld.LearningPathways[0].SourceObject, Is.EqualTo(restoredWorld.LearningSpaces[0]));
            Assert.That(restoredWorld.LearningPathways[0].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningPathways[1].SourceObject, Is.EqualTo(restoredWorld.PathWayConditions[0]));
            Assert.That(restoredWorld.LearningPathways[1].TargetObject, Is.EqualTo(restoredWorld.LearningSpaces[1]));
            Assert.That(restoredWorld.LearningPathways[2].SourceObject, Is.EqualTo(restoredWorld.LearningSpaces[1]));
            Assert.That(restoredWorld.LearningPathways[2].TargetObject, Is.EqualTo(restoredWorld.PathWayConditions[1]));

            Assert.That(initialWorldId, Is.Not.EqualTo(restoredWorld.Id));
            Assert.That(initialSpace1Id, Is.Not.EqualTo(restoredWorld.LearningSpaces.First().Id));
            Assert.That(initialSpace2Id, Is.Not.EqualTo(restoredWorld.LearningSpaces.Last().Id));
            Assert.That(initialTopicId, Is.Not.EqualTo(restoredWorld.Topics.First().Id));
            Assert.That(initialCondition1Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.First().Id));
            Assert.That(initialCondition2Id, Is.Not.EqualTo(restoredWorld.PathWayConditions.Last().Id));
        });
    }

    [Test]
    public void Persistence_SaveAndLoadSpace_File_ObjectsAreEquivalent()
    {
        var space = PersistEntityProvider.GetLearningSpace();
        var initialSpaceId = space.Id;
        var content = PersistEntityProvider.GetFileContent();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var initialElementId = element.Id;
        space.LearningSpaceLayout.LearningElements[0] = element;
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningSpacePe>(fileSystem: mockFileSystem);

        systemUnderTest.SaveToDisk(space, FilePath);
        var restoredSpace = systemUnderTest.LoadFromDisk(FilePath);

        restoredSpace.Should().BeEquivalentTo(space, options => options.Excluding(obj => obj.Id)
            .For(obj => obj.LearningSpaceLayout.ContainedLearningElements)
            .Exclude(obj => obj.Id)
            .Excluding(obj => obj.LearningSpaceLayout.LearningElements[0].Id)
            .WithTracing());
        Assert.That(initialSpaceId, Is.Not.EqualTo(restoredSpace.Id));
    }

    [Test]
    public void Persistence_SaveAndLoadSpace_File_WithAllElementTypes_ObjectsAreEquivalent()
    {
        var space = new LearningSpacePe("Name", "Description", 5, Theme.CampusAschaffenburg)
        {
            LearningSpaceLayout =
            {
                LearningElements = GetAllLearningElementTypes()
                    .Select((e, i) => new KeyValuePair<int, ILearningElementPe>(i, e))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            }
        };

        var systemUnderTest = CreateTestableFileSaveHandler<LearningSpacePe>();

        systemUnderTest.SaveToDisk(space, FilePath);
        var restoredSpace = systemUnderTest.LoadFromDisk(FilePath);

        restoredSpace.Should().BeEquivalentTo(space, options => options
            .Excluding(obj => obj.Id)
            .For(obj => obj.LearningSpaceLayout.ContainedLearningElements)
            .Exclude(obj => obj.Id)
            .Excluding(obj => obj.LearningSpaceLayout.LearningElements[0].Id)
            .Excluding(obj => obj.LearningSpaceLayout.LearningElements[1].Id)
        );
    }

    [Test]
    public void Persistence_SaveAndLoadElement_File_ObjectsAreEquivalent()
    {
        var content = PersistEntityProvider.GetFileContent();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var initialElementId = element.Id;
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningElementPe>(fileSystem: mockFileSystem);

        systemUnderTest.SaveToDisk(element, FilePath);
        var restoredElement = systemUnderTest.LoadFromDisk(FilePath);

        restoredElement.Should().BeEquivalentTo(element, options => options.Excluding(obj => obj.Id));
    }

    [Test]
    [TestCaseSource(nameof(GetAllLearningElementTypes))]
    public void Persistence_SaveAndLoadElement_File_SerializationWorksForEveryType(LearningElementPe lep)
    {
        var systemUnderTest = CreateTestableFileSaveHandler<LearningElementPe>();

        systemUnderTest.SaveToDisk(lep, "element.aef");
        var restoredLep = systemUnderTest.LoadFromDisk("element.aef");

        restoredLep.Should().BeEquivalentTo(lep, options => options.Excluding(obj => obj.Id));
    }

    static ILearningElementPe[] GetAllLearningElementTypes()
    {
        var fileContent = PersistEntityProvider.GetFileContent();
        var linkContent = PersistEntityProvider.GetLinkContent();
        return new ILearningElementPe[]
        {
            PersistEntityProvider.GetLearningElement(append: "FileContent", content: fileContent),
            PersistEntityProvider.GetLearningElement(append: "LinkContent", content: linkContent),
        };
    }

    [Test]
    public void SaveAndLoadWorld_WithExactSameElementInTwoSpaces_ElementIsEqualObject()
    {
        var content = PersistEntityProvider.GetFileContent();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var space1 = new LearningSpacePe("Name", "Description", 5, Theme.CampusAschaffenburg,
            PersistEntityProvider.GetLearningOutcomeCollection(),
            new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>
            {
                {
                    0,
                    element
                }
            }, new Dictionary<int, ILearningElementPe>(), FloorPlanEnum.R_20X30_8L));
        var space2 = new LearningSpacePe("Name", "Description", 5, Theme.CampusAschaffenburg,
            PersistEntityProvider.GetLearningOutcomeCollection(),
            new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>
            {
                {
                    0,
                    element
                }
            }, new Dictionary<int, ILearningElementPe>(), FloorPlanEnum.R_20X30_8L));
        var world = PersistEntityProvider.GetLearningWorld(learningSpaces: new List<LearningSpacePe>
            { space1, space2 });

        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem: mockFileSystem);

        systemUnderTest.SaveToDisk(world, "foobar.txt");

        var actual = systemUnderTest.LoadFromDisk("foobar.txt");

        Assert.That(actual.LearningSpaces[0].LearningSpaceLayout.ContainedLearningElements.First(),
            Is.EqualTo(actual.LearningSpaces[1].LearningSpaceLayout.ContainedLearningElements.First()));
    }

    [Test]
    public void SaveAndLoadWorld_WithTwoEquivalentElementsInTwoSpaces_ElementIsNotEqualObject()
    {
        var content = PersistEntityProvider.GetFileContent();
        var element1 = PersistEntityProvider.GetLearningElement(content: content);
        var element2 = PersistEntityProvider.GetLearningElement(content: content);
        var space1 = new LearningSpacePe("Name", "Description", 5, Theme.CampusAschaffenburg,
            PersistEntityProvider.GetLearningOutcomeCollection(),
            new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>
            {
                {
                    0,
                    element1
                }
            }, new Dictionary<int, ILearningElementPe>(), FloorPlanEnum.R_20X30_8L));
        var space2 = new LearningSpacePe("Name", "Description", 5, Theme.CampusAschaffenburg,
            PersistEntityProvider.GetLearningOutcomeCollection(),
            new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>
            {
                {
                    0,
                    element2
                }
            }, new Dictionary<int, ILearningElementPe>(), FloorPlanEnum.R_20X30_8L));
        var world = PersistEntityProvider.GetLearningWorld(learningSpaces: new List<LearningSpacePe>
            { space1, space2 });

        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem: mockFileSystem);

        systemUnderTest.SaveToDisk(world, "foobar.txt");

        var actual = systemUnderTest.LoadFromDisk("foobar.txt");

        Assert.That(actual.LearningSpaces[0].LearningSpaceLayout.ContainedLearningElements.First(),
            Is.Not.EqualTo(actual.LearningSpaces[1].LearningSpaceLayout.ContainedLearningElements.First()));
    }

    [Test]
    public void SaveAndLoadWorld_WithAdaptivityContentElement_RestoresCorrectly()
    {
        var content = PersistEntityProvider.GetAdaptivityContentFullStructure();
        var element = PersistEntityProvider.GetLearningElement(content: content);
        var space1 = new LearningSpacePe("Name", "Description", 5, Theme.CampusAschaffenburg,
            PersistEntityProvider.GetLearningOutcomeCollection(),
            new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>
            {
                {
                    0,
                    element
                }
            }, new Dictionary<int, ILearningElementPe>(), FloorPlanEnum.R_20X30_8L));
        var world = PersistEntityProvider.GetLearningWorld(learningSpaces: new List<LearningSpacePe>
            { space1 });

        var mockFileSystem = new MockFileSystem();

        var systemUnderTest = CreateTestableFileSaveHandler<LearningWorldPe>(fileSystem: mockFileSystem);

        systemUnderTest.SaveToDisk(world, "foobar.txt");

        var actual = systemUnderTest.LoadFromDisk("foobar.txt");

        var actualContent =
            actual.LearningSpaces.First().LearningSpaceLayout.ContainedLearningElements.First().LearningContent as
                IAdaptivityContentPe;
        var actualQuestion = actualContent.Tasks.First().Questions.First() as MultipleChoiceSingleResponseQuestionPe;
        Assert.That(actualQuestion.Choices.First(), Is.EqualTo(actualQuestion.CorrectChoice));
        Assert.That(actualQuestion.Choices.First(), Is.EqualTo(actualQuestion.CorrectChoices.First()));

        actual.Should().BeEquivalentTo(
            world, options => options.IgnoringCyclicReferences()
                .Excluding(obj => obj.Id)
                .For(obj => obj.LearningSpaces).Exclude(obj => obj.Id)
                .For(obj => obj.LearningSpaces).Exclude(obj => obj.InBoundObjects)
                .For(obj => obj.LearningSpaces).Exclude(obj => obj.OutBoundObjects)
                .For(obj => obj.LearningSpaces).Exclude(obj => obj.AssignedTopic)
                .For(obj => obj.Topics).Exclude(obj => obj.Id)
                .For(obj => obj.PathWayConditions).Exclude(obj => obj.Id)
                .For(obj => obj.PathWayConditions).Exclude(obj => obj.InBoundObjects)
                .For(obj => obj.PathWayConditions).Exclude(obj => obj.OutBoundObjects)
                .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.Id)
                .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.InBoundObjects)
                .For(obj => obj.ObjectsInPathWaysPe).Exclude(obj => obj.OutBoundObjects)
                .For(obj => obj.LearningSpaces).For(obj => obj.LearningSpaceLayout.ContainedLearningElements)
                .Exclude(obj => obj.Id)
                .Excluding(obj => obj.LearningPathways)
                .Excluding(obj => obj.LearningSpaces[0].LearningSpaceLayout.LearningElements[0].Id)
        );

        actualContent.Should().BeEquivalentTo(content, options =>
        {
            options.For(content => content.Tasks).Exclude(task => task.Id);
            options.For(content => content.Tasks).For(task => task.Questions).Exclude(question => question.Id);
            options.For(content => content.Tasks).For(task => task.Questions).For(question => question.Rules)
                .Exclude(rule => rule.Action.Id);
            return options;
        });
    }

    private XmlFileHandler<T> CreateTestableFileSaveHandler<T>(ILogger<XmlFileHandler<T>>? logger = null,
        IFileSystem? fileSystem = null) where T : class
    {
        logger ??= Substitute.For<ILogger<XmlFileHandler<T>>>();
        return fileSystem == null ? new XmlFileHandler<T>(logger) : new XmlFileHandler<T>(logger, fileSystem);
    }
}