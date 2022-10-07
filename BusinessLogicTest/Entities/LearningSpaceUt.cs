using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningSpaceUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var url = "url";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", content1, "url","pupup", "g","h",LearningElementDifficultyEnum.Easy, null, 17, 6, 23);
        var ele2 = new LearningElement("z", "zz", content2,"url","baba", "z","zz", LearningElementDifficultyEnum.Medium, null, 444, 9,double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, requiredPoints, 
            learningElements, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElements));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", content1,"url", "pupup", "g","h",LearningElementDifficultyEnum.Easy, null, 17,90, 23);
        var ele2 = new LearningElement("z", "zz", content2,"url","baba", "z","zz", LearningElementDifficultyEnum.Medium, null, 444,9, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, requiredPoints, 
            learningElements, positionX, positionY);

        var learningSpaceMemento = systemUnderTest.GetMemento();
        
        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var authorsChanged = "sdfg";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var positionXChanged = 10f;
        var positionYChanged = 14f;
        var content1Changed = new LearningContent("a", "b", new byte[]{0x05,0x06});
        var content2Changed = new LearningContent("w", "e", new byte[]{0x08,0x02});
        var ele1Changed = new LearningElement("ab", "bc", content1Changed, "url","pupuper", "ffg","hgg",LearningElementDifficultyEnum.Medium, null, 20,50, 33);
        var ele2Changed = new LearningElement("uu", "iii", content2Changed,"url","lll", "kkk","fff", LearningElementDifficultyEnum.Hard, null, 77,40, 66);
        learningElements.Add(ele1Changed);
        learningElements.Add(ele2Changed);

        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.LearningElements, Contains.Item(ele1Changed));
            Assert.That(systemUnderTest.LearningElements, Contains.Item(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
        });
        
        systemUnderTest.RestoreMemento(learningSpaceMemento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.LearningElements, Does.Not.Contain(ele1Changed));
            Assert.That(systemUnderTest.LearningElements, Does.Not.Contain(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningSpaceMemento_ThrowsException()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", content1, "url","pupup", "g","h",LearningElementDifficultyEnum.Easy, null, 17,13, 23);
        var ele2 = new LearningElement("z", "zz", content2,"url","baba", "z","zz", LearningElementDifficultyEnum.Medium, null, 444,34, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, requiredPoints, 
            learningElements, positionX, positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }

}