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
        var ele1 = new LearningElement("a", content1, "g", "h", LearningElementDifficultyEnum.Easy,
            null, workload: 17, points: 6, positionX: 23);
        var ele2 = new LearningElement("z", content2, "z", "zz",
            LearningElementDifficultyEnum.Medium, null, workload: 444, points: 9, positionX: double.MaxValue);
        var learningElements = new Dictionary<int, ILearningElement>()
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
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
        var ele1 = new LearningElement("a", content1, "g", "h", LearningElementDifficultyEnum.Easy,
            null, workload: 17, points: 6, positionX: 23);
        var ele2 = new LearningElement("z", content2, "z", "zz",
            LearningElementDifficultyEnum.Medium, null, workload: 444, points: 9, positionX: double.MaxValue);
        var learningElements = new Dictionary<int, ILearningElement>()
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var floorPlanName = FloorPlanEnum.Rectangle2X3;

        var systemUnderTest = new LearningSpaceLayout(learningElements, floorPlanName);

        var learningSpaceLayoutMemento = systemUnderTest.GetMemento();

        var content1Changed = new FileContent("c", "d", "");
        var content2Changed = new FileContent("e", "f", "");
        var ele1Changed = new LearningElement("ab", content1Changed, "ffg", "hgg",
            LearningElementDifficultyEnum.Medium, null, workload: 20, points: 50, positionX: 33);
        var ele2Changed = new LearningElement("uu", content2Changed, "kkk", "fff",
            LearningElementDifficultyEnum.Hard, null, workload: 77, points: 40, positionX: 66);
        var learningElementsChanged = new Dictionary<int, ILearningElement>()
        {
            {
                1, ele1Changed
            },
            {
                2, ele2Changed
            }
        };
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
        var ele1 = new LearningElement("a", content1, "g", "h", LearningElementDifficultyEnum.Easy,
            null, workload: 17, points: 6, positionX: 23);
        var ele2 = new LearningElement("z", content2, "z", "zz",
            LearningElementDifficultyEnum.Medium, null, workload: 444, points: 9, positionX: double.MaxValue);
        var learningElements = new Dictionary<int, ILearningElement>()
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
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