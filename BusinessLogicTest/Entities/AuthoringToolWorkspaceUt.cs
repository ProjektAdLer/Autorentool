using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class AuthoringToolWorkspaceUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var world1 = new LearningWorld("foo", "", "f", "", "", "");
        var world2 = new LearningWorld("bar", "", "f", "", "", "");
        var learningWorlds = new List<ILearningWorld>
        {
            world1, world2
        };
        

        var systemUnderTest = new AuthoringToolWorkspace(learningWorlds, new List<ILearningContent>());
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds, Is.EqualTo(learningWorlds));
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var world1 = new LearningWorld("foo", "", "f", "", "", "");
        var world2 = new LearningWorld("bar", "", "f", "", "", "");
        var learningWorlds = new List<ILearningWorld>
        {
            world1, world2
        };

        var systemUnderTest = new AuthoringToolWorkspace(learningWorlds, new List<ILearningContent>());
        
        var memento = systemUnderTest.GetMemento();

        systemUnderTest.LearningWorlds.Remove(world1);
        systemUnderTest.LearningWorlds.Add(new LearningWorld("f", "f", "f", "f", "f", "f"));
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds, Has.Count.EqualTo(2));
        });
        Assert.That(systemUnderTest.LearningWorlds, Contains.Item(world2));
        Assert.That(systemUnderTest.LearningWorlds, Does.Not.Contain(world1));
        
        systemUnderTest.RestoreMemento(memento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds, Has.Count.EqualTo(2));
        });
        Assert.That(systemUnderTest.LearningWorlds, Contains.Item(world1));
        Assert.That(systemUnderTest.LearningWorlds, Contains.Item(world2));
    }

    [Test]
    public void RestoreMemento_NotAuthoringToolWorkspaceMemento_ThrowsException()
    {
        var fakeMemento = Substitute.For<IMemento>();
        
        var systemUnderTest = new AuthoringToolWorkspace(new List<ILearningWorld>(), new List<ILearningContent>());
        
        Assert.That(() => systemUnderTest.RestoreMemento(fakeMemento), Throws.ArgumentException);
    }
}