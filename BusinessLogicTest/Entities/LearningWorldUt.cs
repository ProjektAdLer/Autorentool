using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningWorldUt
{
    [Test]
    public void LearningWorld_Constructor_InitializesAllProperties()
    {
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "eva";
        const string enrolmentKey = "enrolmentkey";
        const string savePath = "C:\\Users\\Ben\\Documents\\test";
        var space1 = new LearningSpace("ff", "ff", 5, Theme.CampusAschaffenburg);
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 2, 3);
        var pathWay = new LearningPathway(space1, pathWayCondition);
        var learningSpaces = new List<ILearningSpace> { space1 };
        var pathWayConditions = new List<PathWayCondition> { pathWayCondition };
        var pathWays = new List<LearningPathway> { pathWay };
        var topic1 = new Topic("topic1");
        var topic2 = new Topic("topic2");
        var topic3 = new Topic("topic3");
        var topics = new List<Topic> { topic1, topic2, topic3 };

        var selectableObjects = new List<ISelectableObjectInWorld> { space1, pathWayCondition, pathWay };

        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals, evaluationLink,
            enrolmentKey,
            savePath: savePath, learningSpaces, pathWayConditions, pathWays, topics);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(language));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.EvaluationLink, Is.EqualTo(evaluationLink));
            Assert.That(systemUnderTest.LearningSpaces, Is.EqualTo(learningSpaces));
            Assert.That(systemUnderTest.PathWayConditions, Is.EqualTo(pathWayConditions));
            Assert.That(systemUnderTest.LearningPathways, Is.EqualTo(pathWays));
            Assert.That(systemUnderTest.Topics, Is.EqualTo(topics));
            Assert.That(systemUnderTest.SelectableWorldObjects, Is.EqualTo(selectableObjects));
            Assert.That(systemUnderTest.UnsavedChanges);
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "eva";
        const string enrolmentKey = "enrolmentkey";
        const string savePath = "C:\\Users\\Ben\\Documents\\test";
        var space1 = new LearningSpace("ff", "ff", 5, Theme.CampusAschaffenburg);
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 2, 3);
        var pathWayConditions = new List<PathWayCondition> { pathWayCondition };
        var learningSpaces = new List<ILearningSpace> { space1 };
        var pathWay = new LearningPathway(space1, pathWayCondition);
        var pathWays = new List<LearningPathway> { pathWay };
        var topic1 = new Topic("topic1");
        var topics = new List<Topic> { topic1 };

        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals, evaluationLink,
            enrolmentKey,
            savePath, learningSpaces, pathWayConditions, pathWays, topics);

        var learningWorldMemento = systemUnderTest.GetMemento();

        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var authorsChanged = "sdfg";
        var languageChanged = "english";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var evaluationLinkChanged = "new evaluation link";
        var savePathChanged = "C:\\Users\\Ben\\Documents\\test2";
        var newElement = EntityProvider.GetLearningElement();
        var space2 = new LearningSpace("gg", "gg", 5, Theme.CampusAschaffenburg);
        var condition2 = new PathWayCondition(ConditionEnum.Or, 2, 1);
        var pathWay2 = new LearningPathway(space2, condition2);
        var topic2 = new Topic("topic2");


        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Language = languageChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.EvaluationLink = evaluationLinkChanged;
        systemUnderTest.SavePath = savePathChanged;
        systemUnderTest.LearningSpaces.Add(space2);
        systemUnderTest.PathWayConditions.Add(condition2);
        systemUnderTest.LearningPathways.Add(pathWay2);
        systemUnderTest.Topics.Add(topic2);
        systemUnderTest.UnplacedLearningElements.Add(newElement);


        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Language, Is.EqualTo(languageChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.EvaluationLink, Is.EqualTo(evaluationLinkChanged));
            Assert.That(systemUnderTest.SavePath, Is.EqualTo(savePathChanged));
            Assert.That(systemUnderTest.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.LearningSpaces[0], Is.EqualTo(space1));
            Assert.That(systemUnderTest.LearningSpaces[1], Is.EqualTo(space2));
            Assert.That(systemUnderTest.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.PathWayConditions[0], Is.EqualTo(pathWayCondition));
            Assert.That(systemUnderTest.PathWayConditions[1], Is.EqualTo(condition2));
            Assert.That(systemUnderTest.LearningPathways, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.LearningPathways[0], Is.EqualTo(pathWay));
            Assert.That(systemUnderTest.LearningPathways[1], Is.EqualTo(pathWay2));
            Assert.That(systemUnderTest.Topics, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.Topics[0], Is.EqualTo(topic1));
            Assert.That(systemUnderTest.Topics[1], Is.EqualTo(topic2));
        });

        systemUnderTest.RestoreMemento(learningWorldMemento);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(language));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.EvaluationLink, Is.EqualTo(evaluationLink));
            Assert.That(systemUnderTest.SavePath, Is.EqualTo(savePath));
            Assert.That(systemUnderTest.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(systemUnderTest.LearningSpaces[0], Is.EqualTo(space1));
            Assert.That(systemUnderTest.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(systemUnderTest.PathWayConditions[0], Is.EqualTo(pathWayCondition));
            Assert.That(systemUnderTest.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(systemUnderTest.LearningPathways[0], Is.EqualTo(pathWay));
            Assert.That(systemUnderTest.Topics, Has.Count.EqualTo(1));
            Assert.That(systemUnderTest.Topics[0], Is.EqualTo(topic1));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningWorldMemento_ThrowsException()
    {
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        const string evaluationLink = "https://";
        const string enrolmentKey = "enrolmentkey";
        const string savePath = "C:\\Users\\Ben\\Documents\\test";
        var space1 = new LearningSpace("ff", "ff", 5, Theme.CampusAschaffenburg);
        var learningSpaces = new List<ILearningSpace> { space1 };

        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals, evaluationLink,
            enrolmentKey,
            savePath: savePath, learningSpaces);


        var mementoMock = new MementoMock();

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
    }
}