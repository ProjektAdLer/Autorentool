using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class AuthoringToolWorkspaceUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var world1 = new World("foo", "", "f", "", "", "");
        var world2 = new World("bar", "", "f", "", "", "");
        var worlds = new List<World>
        {
            world1, world2
        };

        var systemUnderTest = new AuthoringToolWorkspace(world1, worlds);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SelectedWorld, Is.EqualTo(world1));
            Assert.That(systemUnderTest.Worlds, Is.EqualTo(worlds));
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var world1 = new World("foo", "", "f", "", "", "");
        var world2 = new World("bar", "", "f", "", "", "");
        var worlds = new List<World>
        {
            world1, world2
        };

        var systemUnderTest = new AuthoringToolWorkspace(world1, worlds);
        
        var memento = systemUnderTest.GetMemento();

        systemUnderTest.SelectedWorld = world2;
        systemUnderTest.Worlds.Remove(world1);
        systemUnderTest.Worlds.Add(new World("f", "f", "f", "f", "f", "f"));
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SelectedWorld, Is.EqualTo(world2));
            Assert.That(systemUnderTest.Worlds, Has.Count.EqualTo(2));
        });
        Assert.That(systemUnderTest.Worlds, Contains.Item(world2));
        Assert.That(systemUnderTest.Worlds, Does.Not.Contain(world1));
        
        systemUnderTest.RestoreMemento(memento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SelectedWorld, Is.EqualTo(world1));
            Assert.That(systemUnderTest.Worlds, Has.Count.EqualTo(2));
        });
        Assert.That(systemUnderTest.Worlds, Contains.Item(world1));
        Assert.That(systemUnderTest.Worlds, Contains.Item(world2));
    }

    [Test]
    public void RestoreMemento_NotAuthoringToolWorkspaceMemento_ThrowsException()
    {
        var fakeMemento = Substitute.For<IMemento>();
        
        var systemUnderTest = new AuthoringToolWorkspace(null, new List<World>());
        
        Assert.That(() => systemUnderTest.RestoreMemento(fakeMemento), Throws.ArgumentException);
    }
}