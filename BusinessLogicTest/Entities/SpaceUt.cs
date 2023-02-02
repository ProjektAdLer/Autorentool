using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class SpaceUt
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
        var content1 = new Content("a", "b", "");
        var content2 = new Content("w", "e", "");
        var ele1 = new Element("a", "b", content1, "url","pupup", "g","h",ElementDifficultyEnum.Easy, null, 17, 6, 23);
        var ele2 = new Element("z", "zz", content2,"url","baba", "z","zz", ElementDifficultyEnum.Medium, null, 444, 9,double.MaxValue);
        var elements = new IElement?[] { ele1, ele2 };
        var spaceLayout = new SpaceLayout(){Elements = elements};
        
        var systemUnderTest = new Space(name, shortname, authors, description, goals, requiredPoints, 
            spaceLayout, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.ContainedElements, Is.EqualTo(elements));
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
        var content1 = new Content("a", "b", "");
        var content2 = new Content("w", "e", "");
        var ele1 = new Element("a", "b", content1,"url", "pupup", "g","h",ElementDifficultyEnum.Easy, null, 17,90, 23);
        var ele2 = new Element("z", "zz", content2,"url","baba", "z","zz", ElementDifficultyEnum.Medium, null, 444,9, double.MaxValue);
        var elements = new IElement?[] { ele1, ele2 };
        var spaceLayout = new SpaceLayout(){Elements = elements};
        
        var systemUnderTest = new Space(name, shortname, authors, description, goals, requiredPoints, 
            spaceLayout, positionX, positionY);

        var spaceMemento = systemUnderTest.GetMemento();
        var spaceLayoutMemento = systemUnderTest.SpaceLayout.GetMemento();
        
        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var authorsChanged = "sdfg";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var positionXChanged = 10f;
        var positionYChanged = 14f;
        var content1Changed = new Content("a", "b", "");
        var content2Changed = new Content("w", "e", "");
        var ele1Changed = new Element("ab", "bc", content1Changed, "url","pupuper", "ffg","hgg",ElementDifficultyEnum.Medium, null, 20,50, 33);
        var ele2Changed = new Element("uu", "iii", content2Changed,"url","lll", "kkk","fff", ElementDifficultyEnum.Hard, null, 77,40, 66);
        elements = new IElement?[] { ele1Changed, ele2Changed };

        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;
        systemUnderTest.SpaceLayout.Elements = elements;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.ContainedElements, Contains.Item(ele1Changed));
            Assert.That(systemUnderTest.ContainedElements, Contains.Item(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
        });
        
        systemUnderTest.RestoreMemento(spaceMemento);
        systemUnderTest.SpaceLayout.RestoreMemento(spaceLayoutMemento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.ContainedElements, Does.Not.Contain(ele1Changed));
            Assert.That(systemUnderTest.ContainedElements, Does.Not.Contain(ele2Changed));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotSpaceMemento_ThrowsException()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 5f;
        var positionY = 21f;
        var content1 = new Content("a", "b", "");
        var content2 = new Content("w", "e", "");
        var ele1 = new Element("a", "b", content1, "url","pupup", "g","h",ElementDifficultyEnum.Easy, null, 17,13, 23);
        var ele2 = new Element("z", "zz", content2,"url","baba", "z","zz", ElementDifficultyEnum.Medium, null, 444,34, double.MaxValue);
        var elements = new IElement?[] { ele1, ele2 };
        var spaceLayout = new SpaceLayout(){Elements = elements};
        
        var systemUnderTest = new Space(name, shortname, authors, description, goals, requiredPoints, 
            spaceLayout, positionX, positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }

}