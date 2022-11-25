using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

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
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff", 5);
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 2, 3);
        var pathWay = new LearningPathway(space1, pathWayCondition);
        var learningSpaces = new List<LearningSpace> { space1 };
        var pathWayConditions = new List<PathWayCondition> { pathWayCondition };
        var pathWays = new List<LearningPathway> { pathWay };
        
        var selectableObjects = new List<ISelectableObjectInWorld> { space1, pathWayCondition, pathWay };

        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals,
            learningSpaces, pathWayConditions, pathWays);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(language)); 
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.LearningSpaces, Is.EqualTo(learningSpaces));
            Assert.That(systemUnderTest.PathWayConditions, Is.EqualTo(pathWayConditions));
            Assert.That(systemUnderTest.LearningPathways, Is.EqualTo(pathWays));
            Assert.That(systemUnderTest.SelectableWorldObjects, Is.EqualTo(selectableObjects));
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
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff", 5);
        var learningSpaces = new List<LearningSpace> { space1 };
        
        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals, learningSpaces);

        var learningWorldMemento = systemUnderTest.GetMemento();
        
        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var authorsChanged = "sdfg";
        var languageChanged = "english";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var newContent2 = new LearningContent("w", "e", "");
        var space2 = new LearningSpace("gg", "gg", "gg", "gg", "gg", 5);

        
        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Language = languageChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.LearningSpaces.Add(space2);
        

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Language, Is.EqualTo(languageChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.LearningSpaces[0], Is.EqualTo(space1));
            Assert.That(systemUnderTest.LearningSpaces[1], Is.EqualTo(space2));
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
            Assert.That(systemUnderTest.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(systemUnderTest.LearningSpaces[0], Is.EqualTo(space1));
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
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff", 5);
        var learningSpaces = new List<LearningSpace> { space1 };
        
        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals, learningSpaces);


        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }
}