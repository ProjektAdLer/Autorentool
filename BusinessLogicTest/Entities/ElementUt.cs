using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class ElementUt
{
    [Test]
    public void AutomapperConstructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var content = new Content("a", "b", "");
        var authors = "ben and jerry";
        var url = "url";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = ElementDifficultyEnum.Medium;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new Element(name, shortname, content, url, authors, description, goals, difficulty, null, workload, points, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Parent, Is.Null);
            Assert.That(systemUnderTest.Content, Is.EqualTo(content));
            Assert.That(systemUnderTest.Url, Is.EqualTo(url));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }
    
    [Test]
    public void NormalConstructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new Space("foo", "bar", "", "", "", 3);
        var content = new Content("a", "b", "");
        var url = "url";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = ElementDifficultyEnum.Medium;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new Element(name, shortname, content, url, authors, description, goals,
             difficulty, parent, workload, points, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.Content, Is.EqualTo(content));
            Assert.That(systemUnderTest.Url, Is.EqualTo(url));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }
    
    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new Space("foo", "bar", "", "", "", 4);
        var content = new Content("a", "b", "");
        var url = "url";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = ElementDifficultyEnum.Medium;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new Element(name, shortname, content, url, authors, description, goals,
            difficulty, parent, workload, points, positionX, positionY);

        var elementMemento = systemUnderTest.GetMemento();
        
        var nameChanged = "qwertz";
        var shortnameChanged = "uiop";
        var contentChanged = new Content("b", "c", "");
        var urlChanged = "urlChanged";
        var authorsChanged = "sdfg";
        var descriptionChanged = "changed description";
        var goalsChanged = "new goals";
        var difficultyChanged = ElementDifficultyEnum.Easy;
        var workloadChanged = 10;
        var pointsChanged = 20;
        var positionXChanged = 10f;
        var positionYChanged = 14f;

        systemUnderTest.Name = nameChanged;
        systemUnderTest.Shortname = shortnameChanged;
        systemUnderTest.Content = contentChanged;
        systemUnderTest.Url = urlChanged;
        systemUnderTest.Authors = authorsChanged;
        systemUnderTest.Description = descriptionChanged;
        systemUnderTest.Goals = goalsChanged;
        systemUnderTest.Difficulty = difficultyChanged;
        systemUnderTest.Workload = workloadChanged;
        systemUnderTest.Points = pointsChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortnameChanged));
            Assert.That(systemUnderTest.Content, Is.EqualTo(contentChanged));
            Assert.That(systemUnderTest.Url, Is.EqualTo(urlChanged));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authorsChanged));
            Assert.That(systemUnderTest.Description, Is.EqualTo(descriptionChanged));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goalsChanged));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficultyChanged));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workloadChanged));
            Assert.That(systemUnderTest.Points, Is.EqualTo(pointsChanged));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
        });
        
        systemUnderTest.RestoreMemento(elementMemento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Content, Is.EqualTo(content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotElementMemento_ThrowsException()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new Space("foo", "bar", "", "", "", 4);
        var content = new Content("a", "b", "");
        var url = "url";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = ElementDifficultyEnum.Medium;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new Element(name, shortname, content, url, authors, description, goals,
            difficulty, parent, workload, points, positionX, positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }
}