using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningSpaceLayoutUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var content1 = EntityProvider.GetFileContent();
        var content2 = EntityProvider.GetFileContent(append: "2");
        var ele1 = EntityProvider.GetLearningElement(content: content1);
        var ele2 = EntityProvider.GetLearningElement(content: content2, append: "2");
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var floorPlanName = FloorPlanEnum.R_20X30_8L;

        var systemUnderTest = new LearningSpaceLayout(learningElements, TODO, floorPlanName);

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
        var content1 = EntityProvider.GetFileContent();
        var content2 = EntityProvider.GetFileContent(append: "2");
        var ele1 = EntityProvider.GetLearningElement(content: content1);
        var ele2 = EntityProvider.GetLearningElement(content: content2, append: "2");
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var floorPlanName = FloorPlanEnum.R_20X30_8L;

        var systemUnderTest = new LearningSpaceLayout(learningElements, TODO, floorPlanName);

        var learningSpaceLayoutMemento = systemUnderTest.GetMemento();

        var content1Changed = EntityProvider.GetFileContent(append: "c1");
        var content2Changed = EntityProvider.GetFileContent(append: "c2");
        var ele1Changed = EntityProvider.GetLearningElement(append: "c1", content: content1Changed);
        var ele2Changed = EntityProvider.GetLearningElement(append: "c2", content: content2Changed);
        var learningElementsChanged = new Dictionary<int, ILearningElement>
        {
            {
                1, ele1Changed
            },
            {
                2, ele2Changed
            }
        };
        var floorPlanNameChanged = FloorPlanEnum.L_32X31_10L;

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
        var content1 = EntityProvider.GetFileContent();
        var content2 = EntityProvider.GetFileContent(append: "2");
        var ele1 = EntityProvider.GetLearningElement(content: content1);
        var ele2 = EntityProvider.GetLearningElement(content: content2, append: "2");
        var learningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var floorPlanName = FloorPlanEnum.R_20X30_8L;

        var systemUnderTest = new LearningSpaceLayout(learningElements, TODO, floorPlanName);

        var mementoMock = new MementoMock();

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.RestoreMemento(mementoMock));
        Assert.That(ex!.Message, Is.EqualTo("Incorrect IMemento implementation (Parameter 'memento')"));
    }

    private class MementoMock : IMemento
    {
    }
}