using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningSpaceLayoutUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var content1 = new FileContent("a", "b", "");
        var content2 = new FileContent("w", "e", "");
        var ele1 = new LearningElement("a", "b", content1, "pupup", "g", "h", LearningElementDifficultyEnum.Easy,
            null, 17, 6, 23);
        var ele2 = new LearningElement("z", "zz", content2, "baba", "z", "zz",
            LearningElementDifficultyEnum.Medium, null, 444, 9, double.MaxValue);
        var learningElements = new ILearningElement?[] {ele1, ele2, null, null, null, null};
        var floorPlanName = FloorPlanEnum.Rectangle2X3;

        var systemUnderTest = new LearningSpaceLayout(learningElements, floorPlanName);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FloorPlanName, Is.EqualTo(floorPlanName));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElements));
            Assert.That(systemUnderTest.LearningElements[0], Is.SameAs(ele1));
            Assert.That(systemUnderTest.LearningElements[1], Is.SameAs(ele2));
            Assert.That(systemUnderTest.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(systemUnderTest.ContainedLearningElements, Has.Member(ele1));
            Assert.That(systemUnderTest.ContainedLearningElements, Has.Member(ele2));
        });
    }

    [Test]
    public void GetRestoreMemento_RestoresCorrectMemento()
    {
        var content1 = new FileContent("a", "b", "");
        var content2 = new FileContent("w", "e", "");
        var ele1 = new LearningElement("a", "b", content1, "pupup", "g", "h", LearningElementDifficultyEnum.Easy,
            null, 17, 6, 23);
        var ele2 = new LearningElement("z", "zz", content2, "baba", "z", "zz",
            LearningElementDifficultyEnum.Medium, null, 444, 9, double.MaxValue);
        var learningElements = new ILearningElement?[] {ele1, ele2, null, null, null, null};
        var floorPlanName = FloorPlanEnum.Rectangle2X3;

        var systemUnderTest = new LearningSpaceLayout(learningElements, floorPlanName);

        var learningSpaceLayoutMemento = systemUnderTest.GetMemento();

        var content1Changed = new FileContent("c", "d", "");
        var content2Changed = new FileContent("e", "f", "");
        var ele1Changed = new LearningElement("ab", "bc", content1Changed, "pupuper", "ffg", "hgg",
            LearningElementDifficultyEnum.Medium, null, 20, 50, 33);
        var ele2Changed = new LearningElement("uu", "iii", content2Changed, "lll", "kkk", "fff",
            LearningElementDifficultyEnum.Hard, null, 77, 40, 66);
        var learningElementsChanged = new ILearningElement?[] {null, ele1Changed, ele2Changed, null, null};
        var floorPlanNameChanged = FloorPlanEnum.LShape3L2;

        systemUnderTest.FloorPlanName = floorPlanNameChanged;
        systemUnderTest.LearningElements = learningElementsChanged;

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FloorPlanName, Is.EqualTo(floorPlanNameChanged));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElementsChanged));
            Assert.That(systemUnderTest.LearningElements[1], Is.SameAs(ele1Changed));
            Assert.That(systemUnderTest.LearningElements[2], Is.SameAs(ele2Changed));
            Assert.That(systemUnderTest.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(systemUnderTest.ContainedLearningElements, Has.Member(ele1Changed));
            Assert.That(systemUnderTest.ContainedLearningElements, Has.Member(ele2Changed));
        });

        systemUnderTest.RestoreMemento(learningSpaceLayoutMemento);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.FloorPlanName, Is.EqualTo(floorPlanName));
            Assert.That(systemUnderTest.LearningElements, Is.EqualTo(learningElements));
            Assert.That(systemUnderTest.LearningElements[0], Is.SameAs(ele1));
            Assert.That(systemUnderTest.LearningElements[1], Is.SameAs(ele2));
            Assert.That(systemUnderTest.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(systemUnderTest.ContainedLearningElements, Has.Member(ele1));
            Assert.That(systemUnderTest.ContainedLearningElements, Has.Member(ele2));
        });
    }

    [Test]
    public void RestoreMemento_MementoIsNotLearningSpaceLayoutMemento_ThrowsException()
    {
        var content1 = new FileContent("a", "b", "");
        var content2 = new FileContent("w", "e", "");
        var ele1 = new LearningElement("a", "b", content1, "pupup", "g", "h", LearningElementDifficultyEnum.Easy,
            null, 17, 6, 23);
        var ele2 = new LearningElement("z", "zz", content2, "baba", "z", "zz",
            LearningElementDifficultyEnum.Medium, null, 444, 9, double.MaxValue);
        var learningElements = new ILearningElement?[] {ele1, ele2, null, null, null, null};
        var floorPlanName = FloorPlanEnum.Rectangle2X3;

        var systemUnderTest = new LearningSpaceLayout(learningElements, floorPlanName);

        var mementoMock = new MementoMock();

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
    }
}