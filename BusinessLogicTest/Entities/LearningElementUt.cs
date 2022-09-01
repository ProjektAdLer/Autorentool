using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningElementUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new LearningWorld("foo", "bar", "", "", "", "");
        var content = new LearningContent("a", "b", Array.Empty<byte>());
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var workload = 5;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElement(name, shortname, content, authors, description, goals,
             difficulty, parent, workload, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }
    
    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new LearningWorld("foo", "bar", "", "", "", "");
        var content = new LearningContent("a", "b", new byte[]{0x05,0x06});
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var workload = 5;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElement(name, shortname, content, authors, description, goals,
            difficulty, parent, workload, positionX, positionY);

        var learningElementMemento = systemUnderTest.GetMemento();
        
        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var contentChanged = new LearningContent("b", "c", new byte[] {0x03, 0x04});
        var authorsChanged = "sdfg";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var difficultyChanged = LearningElementDifficultyEnum.Easy;
        var workloadChanged = 10;
        var positionXChanged = 10f;
        var positionYChanged = 14f;

        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.LearningContent = contentChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.Difficulty = difficultyChanged;
        systemUnderTest.Workload = workloadChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(contentChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficultyChanged));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workloadChanged));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
        });
        
        systemUnderTest.RestoreMemento(learningElementMemento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningElementMemento_ThrowsException()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new LearningWorld("foo", "bar", "", "", "", "");
        var content = new LearningContent("a", "b", new byte[]{0x05,0x06});
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var workload = 5;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new LearningElement(name, shortname, content, authors, description, goals,
            difficulty, parent, workload, positionX, positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }
}