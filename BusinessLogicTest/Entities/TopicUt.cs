using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Entities;

[TestFixture]

public class TopicUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name ="topic name";
        
        var systemUnderTest = new Topic(name);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.UnsavedChanges);
        });
    }
    
    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var name = "asdf";

        var systemUnderTest = new Topic(name);

        var topicMemento = systemUnderTest.GetMemento();
        
        var nameChanged = "qwertz";
        systemUnderTest.Name = nameChanged;
        
        Assert.That(systemUnderTest.Name, Is.EqualTo(nameChanged));

        systemUnderTest.RestoreMemento(topicMemento);
        
        Assert.That(systemUnderTest.Name, Is.EqualTo(name));
    }
    
    [Test]
    public void RestoreMemento_MementoIsNotLearningSpaceMemento_ThrowsException()
    {
        var name = "asdf";
        
        var systemUnderTest = new Topic(name);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }
}