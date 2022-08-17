using System;
using System.Collections.Generic;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolTest.Entities;

[TestFixture]
public class LearningSpaceUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h",LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz", LearningElementDifficultyEnum.Medium, 444, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, learningElements,
            positionX, positionY);
        
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
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h",LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz", LearningElementDifficultyEnum.Medium, 444, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, learningElements,
            positionX, positionY);

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
        var ele1Changed = new LearningElement("ab", "bc", "ef",content1Changed, "pupuper", "ffg","hgg",LearningElementDifficultyEnum.Medium, 20, 33);
        var ele2Changed = new LearningElement("uu", "iii", "ooo", content2Changed,"lll", "kkk","fff", LearningElementDifficultyEnum.Hard, 77, 66);
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
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h",LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz", LearningElementDifficultyEnum.Medium, 444, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        
        var systemUnderTest = new LearningSpace(name, shortname, authors, description, goals, learningElements,
            positionX, positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }

}