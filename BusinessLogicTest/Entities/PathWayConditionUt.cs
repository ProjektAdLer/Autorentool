using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]

public class PathWayConditionUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var condition = ConditionEnum.And;
        var positionX = 5f;
        var positionY = 21f;
        
        var systemUnderTest = new PathWayCondition(condition, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Condition, Is.EqualTo(condition));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
            Assert.That(systemUnderTest.UnsavedChanges);
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var condition = ConditionEnum.And;
        var positionX = 5f;
        var positionY = 21f;
        
        var systemUnderTest = new PathWayCondition(condition, positionX, positionY);

        var learningSpaceMemento = systemUnderTest.GetMemento();
        
        var conditionChanged = ConditionEnum.Or;
        var positionXChanged = 10f;
        var positionYChanged = 14f;
        
        systemUnderTest.Condition = conditionChanged;
        systemUnderTest.PositionX = positionXChanged;
        systemUnderTest.PositionY = positionYChanged;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Condition, Is.EqualTo(conditionChanged));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionXChanged));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionYChanged));
        });
        
        systemUnderTest.RestoreMemento(learningSpaceMemento);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Condition, Is.EqualTo(condition));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningSpaceMemento_ThrowsException()
    {
        var condition = ConditionEnum.And;
        var positionX = 5f;
        var positionY = 21f;
        
        var systemUnderTest = new PathWayCondition(condition, positionX, positionY);

        var mementoMock = new MementoMock();
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
        
    }
}