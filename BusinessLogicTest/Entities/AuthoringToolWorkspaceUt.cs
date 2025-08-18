using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class AuthoringToolWorkspaceUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var world1 = EntityProvider.GetLearningWorld(append: "1");
        var world2 = EntityProvider.GetLearningWorld(append: "2");
        var learningWorlds = new List<ILearningWorld>
        {
            world1, world2
        };


        var systemUnderTest = new AuthoringToolWorkspace(learningWorlds);

        Assert.Multiple(() => { Assert.That(systemUnderTest.LearningWorlds, Is.EqualTo(learningWorlds)); });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var world1 = EntityProvider.GetLearningWorld(append: "1");
        var world2 = EntityProvider.GetLearningWorld(append: "2");
        var learningWorlds = new List<ILearningWorld>
        {
            world1, world2
        };

        var systemUnderTest = new AuthoringToolWorkspace(learningWorlds);

        var memento = systemUnderTest.GetMemento();

        systemUnderTest.LearningWorlds.Remove(world1);
        systemUnderTest.LearningWorlds.Add(EntityProvider.GetLearningWorld(append: "3"));

        Assert.Multiple(() => { Assert.That(systemUnderTest.LearningWorlds, Has.Count.EqualTo(2)); });
        Assert.That(systemUnderTest.LearningWorlds, Contains.Item(world2));
        Assert.That(systemUnderTest.LearningWorlds, Does.Not.Contain(world1));

        systemUnderTest.RestoreMemento(memento);

        Assert.Multiple(() => { Assert.That(systemUnderTest.LearningWorlds, Has.Count.EqualTo(2)); });
        Assert.That(systemUnderTest.LearningWorlds, Contains.Item(world1));
        Assert.That(systemUnderTest.LearningWorlds, Contains.Item(world2));
    }

    [Test]
    public void RestoreMemento_NotAuthoringToolWorkspaceMemento_ThrowsException()
    {
        var fakeMemento = Substitute.For<IMemento>();

        var systemUnderTest = new AuthoringToolWorkspace(new List<ILearningWorld>());

        Assert.That(() => systemUnderTest.RestoreMemento(fakeMemento), Throws.ArgumentException);
    }
}