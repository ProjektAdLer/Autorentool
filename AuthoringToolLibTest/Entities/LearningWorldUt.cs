using System;
using System.Collections.Generic;
using AuthoringToolLib.Entities;
using AuthoringToolLib.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolLibTest.Entities;

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
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h",LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz",LearningElementDifficultyEnum.Medium, 444, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<LearningSpace> { space1 };

        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals,
            learningElements, learningSpaces);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(language)); 
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElements));
            Assert.That(systemUnderTest.LearningSpaces, Is.EqualTo(learningSpaces));
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
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h",LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz",LearningElementDifficultyEnum.Medium, 444, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<LearningSpace> { space1 };
        
        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals, learningElements, learningSpaces);

        var learningWorldMemento = systemUnderTest.GetMemento();
        
        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var authorsChanged = "sdfg";
        var languageChanged = "english";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var newContent2 = new LearningContent("w", "e", new byte[]{0x08,0x02});
        var newEle2 = new LearningElement("uu", "iii", "ooo", newContent2,"lll", "kkk","fff", LearningElementDifficultyEnum.Hard, 77, 66);
        var space2 = new LearningSpace("gg", "gg", "gg", "gg", "gg");

        
        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Language = languageChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.LearningElements.Remove(ele2);
        systemUnderTest.LearningElements.Add(newEle2);
        systemUnderTest.LearningSpaces.Add(space2);
        

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Language, Is.EqualTo(languageChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.LearningElements, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.LearningElements[0], Is.EqualTo(ele1));
            Assert.That(systemUnderTest.LearningElements[1], Is.EqualTo(newEle2));
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
            Assert.That(systemUnderTest.LearningElements, Has.Count.EqualTo(2));
            Assert.That(systemUnderTest.LearningElements[0], Is.EqualTo(ele1));
            Assert.That(systemUnderTest.LearningElements[1], Is.EqualTo(ele2));
            Assert.That(systemUnderTest.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(systemUnderTest.LearningSpaces[0], Is.EqualTo(space1));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningSpaceMemento_ThrowsException()
    {
        const string name = "asdf";
        const string shortname = "jkl;";
        const string authors = "ben and jerry";
        const string language = "german";
        const string description = "very cool element";
        const string goals = "learn very many things";
        var content1 = new LearningContent("a", "b", new byte[]{0x01,0x02});
        var content2 = new LearningContent("w", "e", new byte[]{0x02,0x01});
        var ele1 = new LearningElement("a", "b", "e",content1, "pupup", "g","h",LearningElementDifficultyEnum.Easy, 17, 23);
        var ele2 = new LearningElement("z", "zz", "zzz", content2,"baba", "z","zz",LearningElementDifficultyEnum.Medium, 444, double.MaxValue);
        var learningElements = new List<LearningElement> { ele1, ele2 };
        var space1 = new LearningSpace("ff", "ff", "ff", "ff", "ff");
        var learningSpaces = new List<LearningSpace> { space1 };
        
        var systemUnderTest = new LearningWorld(name, shortname, authors, language, description, goals, learningElements, learningSpaces);


        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }
}